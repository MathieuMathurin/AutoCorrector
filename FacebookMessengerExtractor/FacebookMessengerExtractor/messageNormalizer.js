var glob = require('glob'),
    fs = require('fs'),
    rl = require('readline');

var ws = fs.createWriteStream('messages/concat3.txt');
glob("messages/messages-*.txt", function (err, files) {
    var nbFilesEnded = 0;
    files.forEach(function (file) {
        var lineReader = require('readline').createInterface({
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
                process.exit();
            }
        });
    });
});