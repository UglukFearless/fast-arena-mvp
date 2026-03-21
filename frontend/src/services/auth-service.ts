import { ApiSettings } from '@/utils/constants';

export interface IAuthStorage {
    getAccessToken(): string | null;
    setAccessToken(token: string): void;
    cleanAuth(): void;
};

class AuthStorage implements IAuthStorage {
    
    private readonly ACCESS_TOKEN_STORAGE_KEY = 'fastarena_access_token'; 

    getAccessToken(): string | null {
        return localStorage.getItem(this.ACCESS_TOKEN_STORAGE_KEY);
    }

    setAccessToken(token: string): void {
        localStorage.setItem(this.ACCESS_TOKEN_STORAGE_KEY, token);
    }

    cleanAuth(): void {
        localStorage.removeItem(this.ACCESS_TOKEN_STORAGE_KEY);
    }
}

export interface IAuthService {
    isAuthenticated(): boolean;
    getExcistingAccessToken(): string | null;
    setAccessToken(token: string): void;
    cleanAuth(): void;
    checkIsTokenAlive(): Promise<void>;
};

class AuthService implements IAuthService {

    constructor(private authStorage: IAuthStorage) {}

    public isAuthenticated(): boolean {
        const token = this.authStorage.getAccessToken();
        return token !== null;
    }

    public getExcistingAccessToken(): string | null {
        return this.authStorage.getAccessToken();
    }

    public setAccessToken(token: string): void {
        this.authStorage.setAccessToken(token);
    }

    public cleanAuth(): void {
        this.authStorage.cleanAuth();
    }

    public async checkIsTokenAlive(): Promise<void> {
        const token = this.getExcistingAccessToken();

        if (token) {
            let headers = new Headers();
            headers.append('Authorization', 'Bearer ' + token);
            const response = await fetch(ApiSettings.BaseUrl + '/api/health/secret-ping', { headers: headers });

            if (response.status !== 200) {
                this.cleanAuth();
            }
        }
    }
}

const authService = new AuthService(new AuthStorage)

export default authService;