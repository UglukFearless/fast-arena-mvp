import authService from "@/services/auth-service";

const authFetch = {
    fetch(url: RequestInfo, init?: RequestInit): Promise<Response> {
        init = init || {};
        const initHeaders = init.headers || {};
        init.headers = Object.assign(initHeaders, { "Authorization": "Bearer " + authService.getExcistingAccessToken() });
        return window.fetch(url, init);
    }
};

export default authFetch;