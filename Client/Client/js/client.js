var socket;


function ConnectSingleplayer() {
    var singleplayersocket = new WebSocket("ws://localhost:5000/singleplayer");
    socket = singleplayersocket;
    singleplayersocket.onopen = function (e) {
        console.log("Connected to the singleplayer server");
    };

    singleplayersocket.onmessage = function (event) {
        Parser(event.data);
    };

    singleplayersocket.onclose = function (event) {
        if (event.wasClean) {
            alert(`[close] Connection closed cleanly, code=${event.code} reason=${event.reason}`);
        } else {
            alert('[close] Connection died');
        }
    };

    singleplayersocket.onerror = function (error) {
        alert(`[error]: `+error);
    };
}

function ConnectMultiplayer() {
    var multiplayersocket = new WebSocket("ws://localhost:5000/multiplayer");
    socket = multiplayersocket;
    multiplayersocket.onopen = function (e) {
        console.log("Connected to the multiplayer server");
    };

    multiplayersocket.onmessage = function (event) {
        Parser(event.data);
    };

    multiplayersocket.onclose = function (event) {
        if (event.wasClean) {
            alert(`[close] Connection closed cleanly, code=${event.code} reason=${event.reason}`);
        } else {
            alert('[close] Connection died');
        }
    };

    multiplayersocket.onerror = function (error) {
        alert(`[error]: ` + error);
    };
}
