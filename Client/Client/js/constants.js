let socket = new WebSocket("ws://localhost:5000/chess");
var gameid = null;
var id = null;
var currentgame = "";
var FEN = "";
var color = "";
var myturn = null;
const baseboard =
    [
        [0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0],
    ];

const dictionary =
    [
        //White characters
        ["P", "white-pawn"],
        ["B", "white-bishop"],
        ["N", "white-knight"],
        ["R", "white-rook"],
        ["Q", "white-queen"],
        ["K", "white-king"],
        //black characters
        ["p", "black-pawn"],
        ["b", "black-bishop"],
        ["n", "black-knight"],
        ["r", "black-rook"],
        ["q", "black-queen"],
        ["k", "black-king"],
    ];

class Message {
    constructor(opcode, message) {
        this.opcode = opcode;
        this.message = message;
    }
}

var possiblemoves = [];
var possibletargets = [];