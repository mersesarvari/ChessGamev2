function CoordParserFromId(element) {
    return element.split('-')[1];
}

function Logger(pretext, element) {
    console.log(pretext + ":");
    console.log(element);
}

