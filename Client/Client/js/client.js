

socket.onopen = function (e) {
    //alert("[Server] Connected succesfully");
    //alert("Sending to server");
    //socket.send("My name is John");
    //Parser(e.data);
};

socket.onmessage = function (event) {
    //alert(`[message] Data received from server: ${event.data}`);
    //console.log(`[message] Data received from server: ${event.data}`);
    Parser(event.data);
};

socket.onclose = function (event) {
    if (event.wasClean) {
        alert(`[close] Connection closed cleanly, code=${event.code} reason=${event.reason}`);
    } else {
        // e.g. server process killed or network down
        // event.code is usually 1006 in this case
        alert('[close] Connection died');
    }
};

socket.onerror = function (error) {
    alert(`[error]`);
};
