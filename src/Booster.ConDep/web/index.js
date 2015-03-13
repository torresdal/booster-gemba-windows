var os = require("os");

var http = require('http');

http.createServer(function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.end('<h5>Server: ' + os.hostname() + '</h5>');
}).listen(process.env.PORT);
