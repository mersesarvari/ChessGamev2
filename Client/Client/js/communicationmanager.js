function Parser(data) {
    const obj = JSON.parse(data);
    //Join message and recieving ID
    if (obj.Opcode === 0) {
        id = obj.Playerid;
        console.log("Connected to the server");
    }
    //Joined game
    else if (obj.Opcode === 4) {
        gameid = obj.Gameid;

        if (obj.Color == "white") {
            id = obj.Playerid;
            color = obj.Color;
            myturn = true;
        }
        else if (obj.Color == "black") {
            id = obj.Playerid;
            color = obj.Color;
            myturn = false;
        }
        else {
            alert("Recieved invalid color from the server");
        }
        FEN = obj.Fen;

        BoardCreator();
        DrawPieces(FEN);
    }
    //Moving
    else if (obj.Opcode === 5) {
        //Itt kéne checkolni a soundokat.
        MovePiece(obj.OldY + "" + obj.OldX, obj.NewY + "" + obj.NewX);
        if (obj.Fen !== undefined) {
            FEN = obj.Fen;
            console.log(FEN);
        }
        if (obj.Possiblemoves!==undefined) {
            possiblemoves = obj.Possiblemoves;
        }
        ResetPossibleMoves();
        if (myturn) {
            myturn = false;
            console.log("You Finished your turn");
            return;
        }
        else {
            myturn = true;
            console.log("Bot Finished his turn");
            return;
        } 
        
        //SetColor();
    }
    else if (obj.Opcode === 6) {
        //Getting all possible moves with my color
        possiblemoves = obj.Moves;


    }
    //Someone won the game
    else if (obj.Opcode === 8) {
        PlaySound("notify");
        alert(obj.message);
    }
}

function SendData(opcode, data) {
    var msg = new Message(opcode, data);
    var stringmsg = JSON.stringify(msg);
    socket.send(stringmsg);

}