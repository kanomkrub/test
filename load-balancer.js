//vee reverse proxy for loadbalance imagin kub
var args = process.argv.splice(2);
var httpProxy = require('http-proxy');
var http = require('http');


var round_robin_listenPort = 2000;
var sticky_iphash_listenPort = 2001;
var addresses = [
	{target:'http://172.20.36.201:3001'},
	{target:'http://172.20.36.201:3002'}
	
    // {
		// target: 'http://172.20.36.201:8001'
    // },
    // {
		// target: 'http://172.20.36.201:8002'
    // },
    // {
		// target: 'http://172.20.36.201:8003'
    // }
];
console.log('-------------------------------------------------------');
console.log('-available direct targets');
addresses.forEach(function (target) {
	console.log("--"+JSON.stringify(target));
});
console.log('-------------------------------------------------------');

var i = 0;
var rrProxy = httpProxy.createProxyServer({});
var rrServer = http.createServer(function (req, res) {
	console.log(req.url+' redirected to '+JSON.stringify(addresses[i]));
	rrProxy.web(req, res, addresses[i]);
	i = (i + 1) % addresses.length;
});
rrServer.listen(round_robin_listenPort);
console.log("-start round robin loadbalancer on port "+round_robin_listenPort);


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
	var server = Math.abs(hash % addresses.length)-1;
	console.log(req.url+' redirected to '+JSON.stringify(addresses[server]));
	iphProxy.web(req, res, addresses[server]);
});
iphserver.listen(sticky_iphash_listenPort);
console.log("-start sticky(ip-hash) loadbalancer on port "+sticky_iphash_listenPort);