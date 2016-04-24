var glob = require('glob'),
    fs = require('fs'),
    rl = require('readline'),
    _ = require('lodash');

var ws = fs.createWriteStream('messages/concat.txt');
glob("messages/messages-*.txt", function (err, files) {
    var nbFilesEnded = 0;
    files.forEach(function (file) {
        var lineReader = rl.createInterface({
            input: fs.createReadStream(file)
        });
        
        lineReader.on('line', function (line) {
            line = isolateStrings(line);
            if(line != "")
                ws.write(line + '\n');
        });

        lineReader.on('close', function () {
            ++nbFilesEnded;
            if (nbFilesEnded == files.length) {
                console.log('All concatenated');
                process.exit();               
                //console.log('File closed');
                //printWeirdLine();
            }
        });
    });
});

var dictionnaire = {};

var printWeirdLine = function (){
    var ws2 = fs.createWriteStream('dictionnary');
    var lineReader = rl.createInterface({
        input: fs.createReadStream('messages/concat.txt')
    });

    lineReader.on('line', function (line) {        
        line = isolateStrings(line);
        var temp = line.split(' ');
        temp.forEach(function (word) {            
            var w = word.toLowerCase();
            if (dictionnaire[word] == null) {
                dictionnaire[word] = 1;
            } else {
                dictionnaire[word]++;
            }
        });
    });

    lineReader.on('close', function () {               
        var temp = _.toPairs(dictionnaire);
        for (var i = 0; i < temp.length; ++i) {
            fs.appendFile('messages/dictionnary', temp[i][0] + ' : ' + temp[i][1] + "\n", 'utf8', function (err) {             
            });
        }        
    });

};

function isolateStrings(line){
    var newLine = line;
    
    if (newLine.indexOf("http") != -1) {
        return "";
    }
    
    //Isolation des caracteres speciaux de regex
    var ponctuations = ['?', '!', '*'];   
    for (var i = 0; i < ponctuations.length; ++i) {
        var char = ponctuations[i];
        var regex = new RegExp('\\' + char, 'g');
        var match = newLine.match(regex);
        if (match != null) {
            //console.log(line);
            //console.log(match[0]);           
            newLine = newLine.replace(regex, " " + char + " ");
            //console.log(newLine);
        }
    }
    
    //Isolation des points et ... d'une ligne (une suite de point est transformer en ...)
    var multiPointRegex = new RegExp('\\.\\.+', 'g');
    var singlePointRegex = new RegExp('.*\\.', 'g');

    var multiPointMatch = newLine.match(multiPointRegex);
    var singlePointMatch = newLine.match(singlePointRegex);
    
    if (multiPointMatch != null) {
        newLine = newLine.replace(multiPointRegex, " " + "..." + " ");
    } else {
        if (singlePointMatch != null) {
            newLine = newLine.replace(singlePointMatch, " " + "." + " ");
        }
    }
    
    //Isolation des caracteres non speciaux de regex
    var chars =['"'];
    for (var i = 0; i < chars.length; ++i) {
        var char = chars[i];
        var regex = new RegExp(char, 'g');
        var match = newLine.match(regex);
        if (match != null) {
            //console.log(line);
            //console.log(match[0]);           
            newLine = newLine.replace(regex, " " + char + " ");
        }
    }

    return newLine;
}