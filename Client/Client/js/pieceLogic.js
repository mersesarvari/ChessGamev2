
var moving = false;
/* Hightlight the current clicked zone */


var tdElement = null;
var selectedpiece = null;
var pieceselected = false;
var possiblemovesfromposition =null;
//var movetarget = null;
var table = document.getElementById("chess-board");
table.addEventListener('click', function (e) {
    console.log("Ez az én köröm?" + myturn);
    SetColor();
    tdElement = document.getElementById("td-" + e.target.id.split('-')[1]);
    //ha targetelni szeretnék egy bábut
    if (e.target.className.includes(color) && !pieceselected) {
        ResetPossibleMoves();
        pieceselected = true;
        console.log("Piece selected for moving:" + e.target.className);
        selectedpiece = e.target.id.split('-')[1];
        //Be kell tölteni a validmoveokat.
        ColorPossibleMoves(selectedpiece[0], selectedpiece[1]);
        return;
    }
    else {
        console.log("Valami hiba van de majd megoldod..")
        console.log("PieceSelected:" + pieceselected);
        console.log("Target color:" + e.target.className.includes(color));

    }
    //Ha már van kiválasztva bábu és mozogni szeretnénk vele.
    if (pieceselected && myturn)
    {
        var newzone = tdElement.id.split('-')[1];
        var oldzone = selectedpiece;
        //Itt le kell checkolni, hogy az új pozíció valid e.
        if (possiblemoves.filter(x =>
            x.OriginalPosition.X == oldzone[0] &&
            x.OriginalPosition.Y == oldzone[1] &&
            x.NewPosition.X == newzone[0] &&
            x.NewPosition.Y == newzone[1]
            ).length > 0) {
        //Feltétel vége
            console.log("Valid move" + oldzone + newzone + "");
            SendMoveToServer(oldzone[1] + "" + oldzone[0], newzone[1] + "" + newzone[0]);
            pieceselected = false;
            return;

        } else {
            console.log("Invalid move" + oldzone + newzone + "");
            pieceselected = false;
            return;
        }
    }
    else {
        console.log("Vagy nincsen kiválasztva bábu vgay pedig nem a te köröd van."+"Myturn: "+myturn)
    }
}, false);

function SendMoveToServer(oldcoord, newcoord) {
    // Moving the char if the move target is not the original position
    if (oldcoord.length > 1 && newcoord.length > 1) {
        var sendmsg = JSON.stringify(
            {
                Opcode: 4,
                Gameid: gameid,
                OldcoordX: oldcoord[0],
                OldcoordY: oldcoord[1],
                NewcoordX: newcoord[0],
                NewcoordY: newcoord[1],
                Playerid: id
            });
        socket.send(sendmsg);
        console.log("Send Move to the server");
    }
    else {
        console.log("cannot send the coordinates. Coordiantes are invalid");
    }
    // ??? //
    moving = false;
}