import { UserClient, UserDto } from '@/api/clients';
import { ApiSettings } from '@/utils/constants';
import authFetch from '@/utils/http-helper';
import { defineStore } from 'pinia';
import { useHeroStore } from './hero';

const heroStore = useHeroStore();

export const useUserStore = defineStore('user', {
    state: () => ({
        user: null as UserDto | null,
    }),
    actions: {
        async initIfNeed() {
            if (!this.user) {
                const userClient = new UserClient(
                    ApiSettings.BaseUrl,
                    authFetch,
                );
                const userInfo = await userClient.getInfo();
                this.user = userInfo.user as UserDto;

                heroStore.setSelectedHero(this.user.selectedHeroId || null);
                heroStore.setHeroes(userInfo.heroes || []);
            }
        },
        setUser(user: UserDto) {
            this.user = user;
        },
        $reset() {
            this.user = null;
        }, 
    },
});