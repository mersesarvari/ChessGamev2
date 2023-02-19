function Parser(data) {
    const obj = JSON.parse(data);
    //Join message and recieving ID
    if (obj.Opcode === 0) {
        id = obj.Playerid;
    }
    //Joined game
    if (obj.Opcode === 4) {
        gameid = obj.Gameid;
        //Checking white and black

        if (obj.Playerid == id) {
            id = obj.Playerid;
            color = 'white';
            myturn = true;
        }
        else {
            id = obj.Playerid;
            color = 'black';
            myturn = false;
        }
        FEN = obj.Fen;

        BoardCreator();
        DrawPieces(FEN);
    }
    //Moving
    if (obj.Opcode === 5) {
        //Itt kéne checkolni a soundokat.
        MovePiece(obj.OldY + "" + obj.OldX, obj.NewY + "" + obj.NewX);
        if (obj.Possiblemoves!==undefined) {
            possiblemoves = obj.Possiblemoves;
        }
        ResetPossibleMoves();
        //if (myturn) {
        //    myturn = false;
        //    console.log("You Finished your turn");
        //    return;
        //}
        //else {
        //    myturn = true;
        //    console.log("Bot Finished his turn");
        //    return;
        //} 
        
        //SetColor();
    }
    if (obj.Opcode === 6) {
        //Getting all possible moves with my color
        possiblemoves = obj.Moves;


    }
    //Someone won the game
    if (obj.Opcode === 8) {
        PlaySound("notify");
        alert(obj.message);
    }
}

function SendData(opcode, data) {
    var msg = new Message(opcode, data);
    var stringmsg = JSON.stringify(msg);
    socket.send(stringmsg);

}