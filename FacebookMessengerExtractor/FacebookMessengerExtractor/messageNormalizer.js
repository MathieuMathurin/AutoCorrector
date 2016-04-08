var glob = require('glob'),
    fs = require('fs'),
    rl = require('readline'),
    _ = require('lodash');

var ws = fs.createWriteStream('messages/concat3.txt');
glob("messages/messages-*.txt", function (err, files) {
    var nbFilesEnded = 0;
    files.forEach(function (file) {
        var lineReader = rl.createInterface({
            input: fs.createReadStream(file)
        });
        
        lineReader.on('line', function (line) {
            if(line != "")
                ws.write(line + '\n');
        });

        lineReader.on('close', function () {
            ++nbFilesEnded;
            if (nbFilesEnded == files.length) {
                console.log('All concatenated');
                //process.exit();               
                console.log('File closed');
                printWeirdLine();
            }
        });
    });
});
var dictionnaire = {};
var wordCount = 0;
var printWeirdLine = function (){
    var ws2 = fs.createWriteStream('dictionnary');
    var lineReader = rl.createInterface({
        input: fs.createReadStream('messages/concat.txt')
    });

    lineReader.on('line', function (line) {
        //var char = '?';
        //var regex = new RegExp('\\' + char, 'g');
        //var match = line.match(regex);
        //if (match != null) {
        //    //console.log(line);
        //    //console.log(match[0]);           
        //    var newLine = line.replace(regex, " " + char + " ");
        //    console.log(newLine);            
        //}
        var temp = line.split(' ');
        temp.forEach(function (word) {
            ++wordCount;
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