import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4300';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'GRCi',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'http://localhost:5000/',
    redirectUri: baseUrl,
    clientId: 'GRCi_App',
    responseType: 'code',
    scope: 'offline_access GRCi',
    requireHttps: false,
  },
  apis: {
    default: {
      url: 'http://localhost:5000',
      rootNamespace: 'GRCi',
    },
  },
} as Environment;
