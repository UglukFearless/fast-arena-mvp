const apiBaseHost = (process.env.VUE_APP_API_BASE_HOST ?? '').trim().replace(/\/$/, '');
const apiBasePort = (process.env.VUE_APP_API_BASE_PORT ?? '').trim();
const apiBaseUrl = apiBaseHost
    ? `${apiBaseHost}${apiBasePort ? `:${apiBasePort}` : ''}`
    : '';

export const ApiSettings = Object.freeze({
    // Empty base URL means same-origin requests: /api/* via backend proxy.
    BaseUrl: apiBaseUrl,
});