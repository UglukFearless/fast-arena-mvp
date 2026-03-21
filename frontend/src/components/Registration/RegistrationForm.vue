<template>
    <form @submit.prevent="" class="login-form">
        <AppLogo />
        <p class="login-form__title">Зарегистрируйтесь или 
            <router-link class="login-form__title-link" to="/login">войдите</router-link>
        </p>
        <AppInput 
            v-model="login"
            name="login"
            errorMessage="Логин не может быть пустым или содержать пробелы"
            placeholder="Логин"
            :readonly="isFormBusy"
            :invalid="!isLoginValid && isFormHit"
        />
        <AppInput 
            v-model="password"
            name="password"
            type="password"
            errorMessage="Пароль не может быть пустым или содержать пробелы"
            placeholder="Пароль"
            :readonly="isFormBusy"
            :invalid="!isPasswordValid && isFormHit"
        />
        <AjaxAppButton
            :state="state"
            message="Зарегистрироваться"
            class="login-form__button_margin-top"
            @click="tryToRegister"
        />
        <div class="login-form__white-spacer"></div>
    </form>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';

import { AjaxActionState } from '@/model/AjaxActionState';
import { ApiException } from '@/api/clients';

import AjaxAppButton from '@/components/shared/buttons/AjaxAppButton.vue';
import AppLogo from '@/components/shared/AppLogo.vue';
import AppInput from '@/components/shared/AppInput.vue';

import { useAccountStore } from '@/stores/account';


const login = ref('');
const password = ref('');
const isFormHit = ref(false);
const state = ref(AjaxActionState.READY);

const router = useRouter();

const accountStore = useAccountStore();

const isFormBusy = computed(() => state.value === AjaxActionState.BUSY);
const isLoginValid = computed(() => login.value.trim() !== '' && !login.value.includes(' '));
const isPasswordValid = computed(() => password.value.trim() !== '' && !password.value.includes(' '));
const isFormValid = computed(() => isLoginValid.value && isPasswordValid.value);

async function tryToRegister() {
    isFormHit.value = true;
    if (isFormValid.value) {
        state.value = AjaxActionState.BUSY;

        accountStore.register(login.value, password.value)
            .then(() => {
                router.push('/');
            }).catch( (e: ApiException) => {
                if (e.status === 400 || e.status === 404) {
                    alert(e.response);
                } else {
                    console.error(e);
                }
            }).finally(() => {
                state.value = AjaxActionState.READY;
            });
    } else {
        state.value = AjaxActionState.READY;
    }
}

</script>

<style lang="scss">
.login-form {
    width: 530px;
    max-width: 100%;
    height: 430px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    align-self: center;
    gap: 16px;
    padding: 5px;

    &__white-spacer {
        flex-grow: 1;
    }

    &__title {
        font-size: 16px;
        line-height: 24px;
    }

    &__title-link {
        text-decoration: none;
    }
    
    &__button_margin-top {
        min-width: 77px;
        margin-top: 20px;
    }
}
</style>