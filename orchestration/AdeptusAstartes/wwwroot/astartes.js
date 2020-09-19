"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/astarteshub").build();

connection.on("all-data", function (message) {
    var legionsContainer = document.getElementById("legions");

    // clear the child elements
    while (legionsContainer.firstChild) {
        legionsContainer.removeChild(legionsContainer.firstChild);
    }

    var legionTable = document.createElement('table');

    message.legions.forEach((legion) => {
        var row = legionTable.insertRow();
        var nameColumn = row.insertColumn();
        nameColumn.innerHTML = legion.name;
        //`<div>{$legion.name}</div>`);
        console.log(JSON.stringify(legion));
    });

    var textArea = document.getElementById("data-output");
    textArea.value = JSON.stringify(message);
});

connection.start().then(function () {
    console.log('connected to astartes hub');
}).catch(function (err) {
    return console.error(err.toString());
});
