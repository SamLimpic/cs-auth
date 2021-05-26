export const dev = window.location.origin.includes('localhost')
export const baseURL = dev ? 'http://localhost:5001' : ''
export const domain = '{{AUTH DOMAIN}}'
export const audience = '{{AUTH AUDIENCE}}'
export const clientId = '{{AUTH CLIENTID}}'
