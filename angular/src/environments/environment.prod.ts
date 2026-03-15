import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'GRCi',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44355/',
    redirectUri: baseUrl,
    clientId: 'GRCi_App',
    responseType: 'code',
    scope: 'offline_access GRCi',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44355',
      rootNamespace: 'GRCi',
    },
  },
} as Environment;
