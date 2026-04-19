<template>
    <section class="hero-info">
        <div class="hero-info__header">
            <button class="hero-info__back" @click="router.back()">← Назад</button>
            <h1 class="hero-info__title">Информация о герое</h1>
        </div>

        <p v-if="loading" class="hero-info__state-card">
            Загружаем информацию о герое...
        </p>

        <p v-else-if="errorMessage" class="hero-info__state-card hero-info__state-card_error">
            {{ errorMessage }}
        </p>

        <template v-else-if="heroInfo">
            <HeroInfoCard :hero="heroInfo" />
            <HeroInventory
                :hero="heroInfo"
                @equipped="onItemEquipped"
                @unequipped="onItemUnequipped"
            />
            <HeroFightHistory :results="heroInfo.results" />
        </template>
    </section>
</template>

<script setup lang="ts">
import { EquipmentSlotType, HeroClient, HeroInfoDto } from '@/api/clients';
import { ApiSettings } from '@/utils/constants';
import authFetch from '@/utils/http-helper';
import { computed, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import HeroFightHistory from './HeroFightHistory.vue';
import HeroInfoCard from './HeroInfoCard.vue';
import HeroInventory from './HeroInventory.vue';

const route = useRoute();
const router = useRouter();
const heroClient = new HeroClient(ApiSettings.BaseUrl, authFetch);

const heroId = computed(() => route.params.id as string || '');
const heroInfo = ref<HeroInfoDto | null>(null);
const loading = ref(false);
const errorMessage = ref('');

async function loadHeroInfo(id: string) {
    if (!id) {
        heroInfo.value = null;
        errorMessage.value = 'Не указан идентификатор героя.';
        return;
    }

    loading.value = true;
    errorMessage.value = '';

    try {
        heroInfo.value = await heroClient.getInfo(id);
    } catch (error) {
        heroInfo.value = null;
        errorMessage.value = (error as Error).message || 'Не удалось загрузить информацию о герое.';
    } finally {
        loading.value = false;
    }
}

watch(heroId, (id) => loadHeroInfo(id), { immediate: true });

async function reloadHeroInfo() {
    await loadHeroInfo(heroId.value);
}

async function onItemEquipped(payload: { heroItemCellId: string; slot: EquipmentSlotType }) {
    const current = heroInfo.value;
    if (!current) {
        return;
    }

    const itemIndex = current.items.findIndex(i => i.id === payload.heroItemCellId);
    const pocketIndex = current.pockets.findIndex(p => p.slot === payload.slot);

    if (itemIndex < 0 || pocketIndex < 0 || current.pockets[pocketIndex].item) {
        await reloadHeroInfo();
        return;
    }

    const pickedItem = current.items[itemIndex];

    current.items.splice(itemIndex, 1);
    current.pockets[pocketIndex].item = pickedItem;
}

async function onItemUnequipped(payload: { slot: EquipmentSlotType }) {
    const current = heroInfo.value;
    if (!current) {
        return;
    }

    const pocketIndex = current.pockets.findIndex(p => p.slot === payload.slot);
    if (pocketIndex < 0 || !current.pockets[pocketIndex].item) {
        await reloadHeroInfo();
        return;
    }

    const removedItem = current.pockets[pocketIndex].item;
    current.pockets[pocketIndex].item = undefined;
    current.items.push(removedItem);
}
</script>

<style lang="scss" scoped>
.hero-info {
    width: 100%;
    max-width: 960px;
    margin: 0 auto;
    padding: 24px 16px;
    display: flex;
    flex-direction: column;
    gap: 20px;

    &__header {
        display: flex;
        align-items: center;
        gap: 16px;
    }

    &__back {
        display: inline-flex;
        align-items: center;
        padding: 8px 16px;
        border-radius: 8px;
        border: 2px solid #111827;
        background: transparent;
        font-size: 16px;
        font-weight: 600;
        cursor: pointer;
        white-space: nowrap;
        transition: background 0.15s, color 0.15s;

        &:hover {
            background: #111827;
            color: white;
        }
    }

    &__title {
        margin: 0;
        font-size: 32px;
        line-height: 1.1;
    }

    &__state-card {
        margin: 0;
        font-size: 20px;
        background: white;
        border-radius: 12px;
        box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);
        padding: 20px;

        &_error {
            color: #992b2b;
            border: 1px solid #d1a3a3;
        }
    }
}
</style>