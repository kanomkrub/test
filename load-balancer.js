var args = process.argv.splice(2);
var httpProxy = require('http-proxy');
var http = require('http');

var addresses = [
    {
		target: 'http://localhost:8001'
    },
    {
		target: 'http://localhost:8002'
    },
    {
		target: 'http://localhost:8003'
    }
];

console.log('-available direct targets');
addresses.forEach(function (target) {
	console.log(JSON.stringify(target));
});

var i = 0;
var rrProxy = httpProxy.createProxyServer({});
var rrServer = http.createServer(function (req, res) {
	rrProxy.web(req, res, addresses[i]);
	i = (i + 1) % addresses.length;
});
rrServer.listen(5050);
console.log("-start round robin loadbalancer on port 5050")


var iphProxy = httpProxy.createProxyServer({});
var iphserver = http.createServer(function (req, res) {
	var ip = req.headers['x-forwarded-for'] || 
     req.connection.remoteAddress || 
     req.socket.remoteAddress ||
     req.connection.socket.remoteAddress;
	//req.headers['x-forwarded-for'] = ip;
	//res.setHeader('client-ip', ip);
	
	var hash = 0;
	for (i = 0; i < ip.length; i++) {
		char = ip.charCodeAt(i);
		hash = ((hash << 5) - hash) + char;
		hash = hash & hash;
	}
	var server = hash % addresses.length;
	iphProxy.web(req, res, addresses[server]);
});
iphserver.listen(5051);
console.log("-start sticky(ip-hash) loadbalancer on port 5051")