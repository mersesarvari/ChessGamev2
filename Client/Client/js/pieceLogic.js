
var moving = false;
/* Hightlight the current clicked zone */

var selectedpiecezonecolor = null;
var tdElement = null;
var selectedpiece = null;
var pieceselected = false;
var possiblemovesfromposition = null;
//var movetarget = null;
var table = document.getElementById("chess-board");
table.addEventListener('click', function (e) {
    console.log("My turn?" + myturn);
    tdElement = document.getElementById("td-" + e.target.id.split('-')[1]);
    //ha targetelni szeretnék egy bábut
    if (e.target.className.includes(color) && !pieceselected && myturn) {
        console.log("Bábu targetelése");
        ResetPossibleMoves();
        Targeting(e);
        return;
    }
    else if (e.target.className.includes(color) && pieceselected && myturn) {
        console.log("Rossz targetelése");
        ResetPossibleMoves();
        //ha kétszer ugyanazt a bábut targetelem.
        if (selectedpiece == e.target.id.split('-')[1]) {
            ClearTarget();
            return;
        }
        else {
            Targeting(e);
            return;
        }
        
    }
    //Ha már van kiválasztva bábu és mozogni szeretnénk vele.
    Move(e);
    
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
        myturn = false;
        console.log("Send Move to the server");
    }
    else {
        console.log("cannot send the coordinates. Coordiantes are invalid");
    }
    // ??? //
    moving = false;
}

function Targeting(e) {
    SetBoardColors();
    pieceselected = true;
    console.log("Piece selected for moving:" + e.target.className);
    selectedpiece = e.target.id.split('-')[1];
    selectedpiecezonecolor = document.getElementById('td-' + selectedpiece).className;
    document.getElementById('td-' + selectedpiece).className = 'yellow';
    //Be kell tölteni a validmoveokat.
    ColorPossibleMoves(selectedpiece[0], selectedpiece[1]);
}
function ClearTarget(){
    pieceselected = false;
    selectedpiece = null;
    SetBoardColors();
}


function Move(e) {
    if (pieceselected && myturn) {
        var newzone = tdElement.id.split('-')[1];
        var oldzone = selectedpiece;
        //Ha olyan mezőt targetelek ahova léphetek
        if (possiblemoves.filter(x =>
            x.OriginalPosition.X == oldzone[0] &&
            x.OriginalPosition.Y == oldzone[1] &&
            x.NewPosition.X == newzone[0] &&
            x.NewPosition.Y == newzone[1]).length > 0) {
            SendMoveToServer(oldzone[1] + "" + oldzone[0], newzone[1] + "" + newzone[0]);
            pieceselected = false;
            SetBoardColors();
            return;
        }
        //Ha olyan mezőt targetelek ki ahova nem léphetek
        else {
            console.log("C");
            ResetPossibleMoves();
            pieceselected = false;
            selectedpiece = null;
            SetBoardColors();
            return;
            
        }
    }
    else {
        console.log("D");
        ResetPossibleMoves();
        pieceselected = false;
        SetBoardColors();
        return;
    }
}
