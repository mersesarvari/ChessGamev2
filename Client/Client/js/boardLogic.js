//rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w 
function ConvertFromFEN(fenstring) {
    //Többi beállítás hiányzik logic kell ide

    //Alapvető állás konverzió
    fenstring = fenstring.split(' ')[0];
    var _board = baseboard;
    var line = [];
    for (var i = 0; i < fenstring.split('/').length; i++) {
        line = [];
        for (let chars = 0; chars < fenstring.split('/')[i].length; chars++) {
            var current = fenstring.split('/')[i][chars];
            if (current.toUpperCase() === current) {
                current = current.toLowerCase();
            }
            else if(current.toLowerCase() === current) {
                current = current.toUpperCase();
            }
            //CHARACTER IS NUMBER
            //Checking empty row
            if (current == "8") {
                line = ["0", "0", "0", "0", "0", "0", "0", "0"];
                break;
            }
            // Checking empty cells in the row
            else if (current == "1" || current == "2" || current == "3" || current == "4" || current == "5" || current == "6" || current == "7") {
                for (let szorzo = 0; szorzo < parseInt(current); szorzo++) {
                    line.push("0");
                }
            }
            //Checking valid piececodes
            else if ("rnbqkpRNBQKP".includes(current.toLowerCase())) {
                line.push(current + "");
            }
            else {
                throw ("[Error]: cannot parse char: " + current);
            }

        };
        //console.log("Line: " + line.toString());
        _board[i] = line;
    };
    document.getElementById("fenstringholder").innerHTML = fenstring;
    return _board;

};
function ConvertToFen() {
    var current = BOARD;
    //console.log("Board");
    //console.log(BOARD);
    //console.log(current);
    var fenstring = "";
    var numbercounter = 0;
    for (let i = 0; i < 8; i++) {
        for (let j = 0; j < 8; j++) {
            numbercounter = 0;

            while (current[i][j] === '0') {
                numbercounter++;
                j++
            }
            if (numbercounter > 0) {
                fenstring += numbercounter;
            }
            if (current[i][j] !== '0' && current[i][j] !== undefined) {
                fenstring += current[i][j];
            }
        }
        if (i < 7) {
            fenstring += "/"
        }
    }
    Logger("ConvertToFen", fenstring);
    document.getElementById("fenstringholder").innerHTML = fenstring;
    FEN = fenstring;
    return fenstring;
}
function BoardCreator() {
    console.log("You are the " + color + " player");
    //Resetting board
    var brd = document.getElementById("table");
    brd.remove();
    var brd2 = document.createElement("tbody");
    brd2.id = "table";
    document.getElementById('chess-board').appendChild(brd2);

    //Creating rows
    for (let i = 7; i > -1; i--) {
        const tr = document.createElement("tr");
        tr.className = "tr-" + i;
        tr.id = "tr-" + i;
        document.getElementById('table').appendChild(tr);

        //creating columns
        for (let j = 0; j < 8; j++) {
            const td = document.createElement("td");
            td.id = "td-" + j + "" + i;
            //td.className = SetZoneColor(j, i);
            //td.textContent = j + "" + i;
            document.getElementById("tr-" + i).appendChild(td);
            //creating image sources
            const image = document.createElement("div");
            image.id = "div-" + j + i;
            image.className = "free";
            document.getElementById("td-" + j + "" + i).appendChild(image);
        }
    }
    SetBoardColors();
    //Color white
    if (color === "black") {
        flipBoard();
    }
    
}
function SetBoardColors() {
    for (var i = 0; i < 8; i++) {
        for (var j = 0; j < 8; j++) {
            document.getElementById('td-' + i + "" + j).className = SetZoneColor(i, j);
        }
    }
}
function DrawPieces() {
    BOARD = ConvertFromFEN(FEN);
    for (let i = 0; i < 8; i++) {
        for (let j = 0; j < 8; j++) {
            //Clearing wrong positions
            if (BOARD[j][i] === '0') {
                var tmp = document.getElementById('div-' + i + j);
                tmp.className = 'free';
            }
            //Adding Pieces as images
            for (let x = 0; x < dictionary.length; x++) {
                if (BOARD[j][i] === dictionary[x][0]) {
                    //Select image source
                    if (BOARD[j][i] !== "0") {
                        var tmp = document.getElementById('div-' + i + j);
                        tmp.className = dictionary[x][1].toString();
                    }
                }
            }
        }
    }
}

function flipBoard() {
    var board = document.getElementById('chess-board');
    board.style.transform = 'rotate(180deg)';
    var divsintable = table.getElementsByTagName('div');
    for (var i = 0; i < divsintable.length; i++) {
        divsintable[i].style.transform = 'rotate(180deg)';
    }
}
function MovePiece(oldcoord, newcoord)
{
    var original = document.getElementById('div-' + oldcoord);
    var newposdiv = document.getElementById('div-' + newcoord);
    if (newposdiv.className == "free") {
        PlaySound("move");
    }
    else {
        PlaySound("capture");
    }
    newposdiv.className = original.className;
    original.className = "free"; 
}
function SetZoneColor(i, j) {
    //Even
    if ((i % 2 === 0 && j % 2 === 0) || (i % 2 === 1 && j % 2 === 1)) {

        return "dark";
    }
    else {
        return "light";
    }
}

function GetPossibleMoves(fromx, fromy) {
    var PositionstoColor = [];
    for (let i = 0; i < possiblemoves.length; i++) {
        if (possiblemoves[i].OriginalPosition.X == fromx && possiblemoves[i].OriginalPosition.Y == fromy) {
            PositionstoColor.push(possiblemoves[i].NewPosition);
        }
    }
    return PositionstoColor;
}
function ColorPossibleMoves(x, y) {
    x = Number(x);
    y = Number(y);
    var moves = GetPossibleMoves(x, y);

    //console.log(c);
    for (let i = 0; i < moves.length; i++) {
        var currentzone = document.getElementById("td-" + moves[i].X + moves[i].Y);
        //Csak azoknál van option ahol nem áll bábu
        currentzone.classList.add("option");
        //currentzone.classList.remove("free");
        possibletargets.push(currentzone);
    }

}
function ResetPossibleMoves() {
    //var options = document.getElementsByClassName("option");
    for (let i = 0; i < possibletargets.length; i++) {
        //possibletargets[i].classList.add("free");
        possibletargets[i].classList.remove("option");
    }
    possibletargets = [];

}

function PlaySound(type) {
    if (type == "move") {
        var x = document.getElementById("moveAudio");
        x.play();
    }
    if (type == "notify") {
        var x = document.getElementById("notifyAudio");
        x.play();
    }
    if (type == "capture") {
        var x = document.getElementById("captureAudio");
        x.play();
    }
}


