import { defineStore } from 'pinia';
import authService from '@/services/auth-service';
import { AccountClient, UserDto } from '@/api/clients';
import { ApiSettings } from '@/utils/constants';
import { useUserStore } from '@/stores/user';

const userStore = useUserStore();

export const useAccountStore = defineStore('account', {
    actions: {
        async register(login: string, password: string) {
            const authClient = new AccountClient(ApiSettings.BaseUrl);
            const response = await authClient.registration({
                login: login,
                password: password,
            });
            authService.setAccessToken(response.accessToken as string);
            userStore.setUser(response.user as UserDto);
        },
        async login(login: string, password: string) {
            const authClient = new AccountClient(ApiSettings.BaseUrl);
            const response = await authClient.login({
                login: login,
                password: password,
            });
            authService.setAccessToken(response.accessToken as string);
            userStore.setUser(response.user as UserDto);
        },
        async logout() {
            userStore.$reset();
            authService.cleanAuth();
        },
    },
});