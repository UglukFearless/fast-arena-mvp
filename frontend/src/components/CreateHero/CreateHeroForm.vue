<template>
    <form class="create-hero__form" v-on:submit.prevent="createHero">
        <InputGroup name="Портрет">
            <PortraiteInput v-model="portraitId" />
        </InputGroup>
        <InputGroup name="Имя">
            <input class="create-hero__input" required v-model="heroName" name="name" type="text" maxlength="64" />
        </InputGroup>
        <InputGroup name="Пол">
            <SexInput v-model="heroSex" />
        </InputGroup>
        <InputGroup>
            <AjaxAppButton
                :state="state"
                class="create-hero__save-button" 
                message="Создать" 
            />
        </InputGroup>
    </form>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { AjaxActionState } from '@/model/AjaxActionState';
import AjaxAppButton from '@/components/shared/buttons/AjaxAppButton.vue';
import InputGroup from '@/components/CreateHero/InputGroup.vue';
import SexInput from './SexInput.vue';
import PortraiteInput from './PortraiteInput.vue';
import { useHeroStore } from '@/stores/hero';
import { HeroSex } from '@/api/clients';


const state = ref(AjaxActionState.READY);

const portraitId = ref('');
const heroName = ref('');
const heroSex = ref(0);

const heroStore = useHeroStore();
const router = useRouter();

async function createHero() {
    const model = {
        name: heroName.value,
        sex: heroSex.value as HeroSex,
        portraitId: portraitId.value,
    };
    await heroStore.create(model);
    router.push('/');
}

</script>

<style lang="scss">
.create-hero {
    &__form {
        display: flex;
        justify-content: center;
        flex-direction: column;
        gap: 12px;
    }

    &__save-button {
        align-self: center;
        font-size: 16px;
        font-weight: bold;
        margin: 4px auto;
        padding: 4px 32px;
        cursor: pointer;
    }

    &__input {
        flex: 1;
        font-size: 16px;
        padding: 4px 16px;
        max-width: 50%;
    }
}
</style>