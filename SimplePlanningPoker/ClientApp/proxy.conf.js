const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:60282';

console.log(`Proxying to ${target}`);
const PROXY_CONFIG = [
  {
    context: [
      "/api",
      "/weatherforecast"
    ],
    proxyTimeout: 10000,
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  },
  {
    context: [
      "/roomhub",
    ],
    target: target,
    secure: false,
    ws: true,
    logLevel: 'debug'
  }
]

module.exports = PROXY_CONFIG;

