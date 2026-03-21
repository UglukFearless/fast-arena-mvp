<template>
    <header class="header">
        <span class="header__account-name">{{ userLogin }}</span>
        <h1 class="header__main-title">
            <AppLogo class="header__image" />
            <span class="header__title">Fast Arena</span>
        </h1>
        <AppButton class="header__logout-button" @click="logout">Выход</AppButton>
        <nav class="header__navigation">
            <a class="header__nav-link" href="/" @click="goBarraks($event)">Казарма</a>
            <a class="header__nav-link" href="/arena.html" @click="goArena($event)">Арена</a>
            <a class="header__nav-link" href="/shop.html" @click="goShop($event)">Магазин</a>
            <a class="header__nav-link" href="/statistic.html" @click="goStatistic($event)">Статистика</a>
        </nav>
    </header>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRouter } from 'vue-router';

import { useAccountStore } from '@/stores/account';
import AppButton from './buttons/AppButton.vue';
import AppLogo from './AppLogo.vue';
import { useUserStore } from '@/stores/user';
import { useHeroStore } from '@/stores/hero';

const router = useRouter();
const accountStore = useAccountStore();
const userStore = useUserStore();
const heroStore = useHeroStore();

userStore.initIfNeed();

const userLogin = computed(() => userStore.user?.login);

function logout() {
    accountStore.logout();
    router.push('/login');
}

function goArena(e: Event) {
    e.preventDefault();
    if (!heroStore.selectedHeroId) {
        alert('Необходимо выбрать героя, чтобы отправиться на арену.');
        return;
    } else {
        router.push('/arena');
    }
}

function goShop(e: Event) {
    e.preventDefault();
    if (!heroStore.selectedHeroId) {
        alert('Необходимо выбрать героя, чтобы отправиться в магазин.');
        return;
    } else {
        router.push('/shop');
    }
}
function goBarraks(e: Event) {
    e.preventDefault();
    router.push('/');
}

function goStatistic(e: Event) {
    e.preventDefault();
    router.push('/statistic');
}

</script>

<style lang="scss">
.header {
    position: relative;
    padding: 16px 0;
    box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);

    &__image {
        height: 0.85em;
        padding-right: 8px;
    }

    &__account-name {
        position: absolute;
        left: 32px;
        top: 1.25em;
        font-size: 26px;
        font-weight: bold;
        padding: 4px 32px;
    }

    &__logout-button {
        position: absolute;
        right: 32px;
        top: 2.5em;
        max-width: 150px;
        font-size: 16px;
        font-weight: bold;
        padding: 4px 32px;
        cursor: pointer;
    }

    &__main-title {
        width: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        text-align: center;
        margin: 0;
    }

    &__navigation {
        display: flex;
        justify-content: center;
        padding: 10px 0 0 0;
    }

    &__nav-link {
        text-decoration: none;
        padding: 4px 12px;
        font-size: 18px;
        color: grey;
        font-weight: bold;
        border: 1px solid #aaa;
        background: white;
    }

    &__nav-link {
        &:link, &:visited {
            color: black;
        }

        &:hover {
            transform: scale(1.1);
        }
    }
}
</style>