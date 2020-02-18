
//Start watering and limit it to 10 seconds unless the Stop is clicked.
function startWatering(btnId, plantId) {
    var restLbl = btnId.replace('start_btn', 'status_lbl');
    var status = document.getElementById(restLbl).innerText;
    if (status != 'Resting...' && status != 'Watering...') {
        var timeleft = 10;
        document.getElementById(btnId).setAttribute('hidden', 'hidden');
        var stopBtnId = btnId.replace('start_btn', 'stop_btn');

        changeStatus(restLbl, 'Watering...', 'blue');

        document.getElementById(stopBtnId).removeAttribute('hidden');
        var index = getIndex(btnId);

        //Set a value to a global variable defined in code-behind.
        window['p' + index] = startTimer(btnId, timeleft, plantId);
    }
}

//Save a watering to the database
function saveWatering(id, plantId, isStart) {
    var dataValue = "{plantId: " + plantId + "}";
    $.ajax({
        type: "POST",
        url: "WaterPlants.aspx/SaveWatering",
        data: dataValue,
        contentType: 'application/json',
        dataType: 'json',
        error: function () {
            //Display an error if the post doesn't go well.
            alert('The watering attempt encountered an error, please try again.');
        },
        success: function (result) {
            stopWatering(id, isStart, result.d);
        }
    });
}

//Stop watering
function stopWatering(id, isStart, result) {
    stopTimer(id);
    var btnId = id;
    var restLbl = '';
    if (isStart) {
        btnId = id.replace('start_btn', 'stop_btn');
        restLbl = id.replace('start_btn', 'status_lbl');
    }
    else {
        restLbl = id.replace('stop_btn', 'status_lbl');
    }

    document.getElementById(btnId).setAttribute('hidden', 'hidden');

    changeStatus(restLbl, 'Resting...', 'black');

    setTimeout(function () {

        changeStatus(restLbl, 'Good.', 'green');
        var progBar = restLbl.replace('status_lbl', 'waterBar_bar');
        document.getElementById(progBar).value = 0;
        var startBtn = id;
        if (!isStart) {
            startBtn = id.replace('stop_btn', 'start_btn');
        }
        document.getElementById(startBtn).removeAttribute('hidden');
    }, 30000);
    changeWaterDateAndCount(btnId, result);
}


//Change the Last Watered date.
function changeWaterDateAndCount(id, result) {
    var waterDate = id.replace('stop_btn', 'lastTime_txt');
    var waterCount = id.replace('stop_btn', 'waterCount_lbl');
    document.getElementById(waterDate).innerText = result.split(',')[0];
    document.getElementById(waterCount).innerText = result.split(',')[1];
}

//Stop watering timer if the Stop button is clicked.
function stopTimer(id) {
    var index = getIndex(id);
    clearInterval(window['p' + index]);
}

//Start timer to 10 seconds
function startTimer(btnId, timeleft, plantId) {
    downloadTimer = setInterval(function () {
        if (timeleft <= 0) {
            stopTimer(btnId);
        }
        var barId = btnId.replace('start_btn', 'waterBar_bar');
        document.getElementById(barId).value = 10 - timeleft;
        timeleft -= 1;
        if (timeleft == -1) {
            saveWatering(btnId, plantId, true);
        }
    }, 1000);
    return downloadTimer;
}

//Get the Plant index from its ID
function getIndex(id) {
    return id.split("_").pop();
}

//Save selected plants to water
function selectPlant(id, plantId) {
    var chk = document.getElementById(id);
    var multiTxt = document.getElementById('multiWatering_txt');
    var methodValue = id.replace('select_chk', 'start_btn') + '-' + plantId + ',';
    if (chk.checked) {
        multiTxt.value += methodValue;
    }
    else {
        multiTxt.value = multiTxt.value.replace(methodValue, '');
    }
    checkAllCheckboxes();
}

//Start watering multiple plants at once.
function multiWatering() {
    var text = document.getElementById('multiWatering_txt').value;
    if (text == '') {
        alert('Please select one plant at least.');
        return;
    }
    var list = text.split(',');
    for (var i = 0; i < list.length; i++) {
        if (list[i] != '') {
            startWatering(list[i].split('-')[0], list[i].split('-')[1]);
        }
    }
    clearCheckboxes();
}

//Clear the selected plants after watering gets started.
function clearCheckboxes() {
    var text = document.getElementById('multiWatering_txt').value;
    var list = text.split(',');
    for (var i = 0; i < list.length; i++) {
        if (list[i] != '') {
            var chk = list[i].split('-')[0].replace('start_btn', 'select_chk');
            document.getElementById(chk).checked = false;
        }
    }
    document.getElementById('multiWatering_txt').value = '';
    document.getElementById('Plants_grd_selectAll_chk').checked = false;
}

//Change the Water Status per each request.
function changeStatus(id, statusText, color) {
    document.getElementById(id).innerText = statusText;
    document.getElementById(id).style = "color: " + color + ";";
}

//Select All plants
function selectAllPlants(selectAllId) {
    var isChecked = document.getElementById(selectAllId).checked;
    var grid = document.getElementById('Plants_grd');
    var inputs = grid.getElementsByTagName('input');
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].id.indexOf("select_chk") != -1) {
            //Add new checked only.
            if (!isChecked || (isChecked && !inputs[i].checked)) {
                inputs[i].checked = isChecked;
                var plantIdTxt = inputs[i].id.replace('select_chk', 'plantId_lbl');
                var plantId = document.getElementById(plantIdTxt).innerText;
                selectPlant(inputs[i].id, plantId);
            }
        }
    }

}

//Check or uncheck the SelectAll checkbox.
function checkAllCheckboxes() {
    var checkedArr = [];
    var grid = document.getElementById('Plants_grd');
    var inputs = grid.getElementsByTagName('input');
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].id.indexOf("select_chk") != -1) {
            checkedArr.push(inputs[i].checked);
        }
    }
    //All checkboxes are check, then check the SelectAll.
    if (checkedArr.includes(true) && !checkedArr.includes(false)) {
        document.getElementById('Plants_grd_selectAll_chk').checked = true;
        return;
    }
    //All checkboxes are uncheck, then uncheck the SelectAll and clear the values.
    if (checkedArr.includes(false) && !checkedArr.includes(true)) {
        document.getElementById('Plants_grd_selectAll_chk').checked = false;

        //Possibly not needed, just to ensure no values are left.
        document.getElementById('multiWatering_txt').value = '';
        return;
    }
    //If one of the checkboxes is unchecked, uncheck the SelectAll.
    if (checkedArr.includes(false) && checkedArr.includes(true)) {
        document.getElementById('Plants_grd_selectAll_chk').checked = false;
        return;
    }

}