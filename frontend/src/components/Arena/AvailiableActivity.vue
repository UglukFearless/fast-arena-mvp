<template>
    <div class="available-activity">
        <h2 class="available-activity__title"> {{ activity.title }} </h2>
        <div class="available-activity__info">
            <img class="available-activity__image" :src="activity.iconUrl" />
            <div class="available-activity__props">
                <div class="available-activity__props-item">
                    <span class="available-activity__props-name">Опсность:</span>
                    <span class="available-activity__props-value">{{ activity.dangerLevelName }}</span>
                </div>
                <div class="available-activity__props-item">
                    <span class="available-activity__props-name">Награда:</span>
                    <span class="available-activity__props-value">{{ activity.awardLevelName }}</span>
                </div>
                <div class="available-activity__props-item">
                    <span class="available-activity__props-name">Тип:</span>
                    <span class="available-activity__props-value">{{ activity.typeName }}</span>
                </div>
            </div>
        </div>
        <div class="available-activity__actions">
            <AppButton :title="activity.description" @click="goTo">
                {{ activity.activationTitle }}
            </AppButton>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, PropType } from 'vue';
import { ActivityActionType, ActivityDto } from "@/api/clients";

import AppButton from '@/components/shared/buttons/AppButton.vue';
import { useActivitySessionStore } from '@/stores/activity-session';
import router, { getRoutePathByActionType } from '@/router';

const activitySessionStore = useActivitySessionStore();


const props = defineProps({
    activity: {
        required: true,
        type: Object as PropType<ActivityDto>,
    }
});

const session = computed(() => activitySessionStore.session);

async function goTo() {
    await activitySessionStore.start(props.activity.id);
    const nextRoute = getRoutePathByActionType(session.value?.currentAction.type as ActivityActionType);
    router.push(nextRoute);
}

</script>

<style lang="scss">
.available-activity {
    background: white;
    margin: 12px;
    padding: 12px;
    box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);
    border-radius: 8px;
    max-width: 94vw;
    width: 340px;
    display: inline-flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;

    &:hover {
        box-shadow: 0px 0px 2px rgba(155, 155, 155, 0.6);
    }

    &__title {
        font-size: 18px;
        margin-bottom: 8px;
    }

    &__info {
        display: flex;
        justify-content: center;
        align-items: center;
        max-width: 100%;
    }

    &__image {
        width: 64px;
        margin: 0 12px;
    }

    &__props {
        display: flex;
        max-width: 160px;
        flex-direction: column;
        margin: 0 12px;
        flex-wrap: wrap;

        &-item {
            display: flex;
            align-items: center;
            font-size: 18px;
        }

        &-name {
            font-weight: bold;
            flex: 1;
            padding: 2px 8px 2px 0;
        }

        &-value {
            flex: 2;
        }
    }

    &__actions {
        width: 100%;
        padding-top: 10px;
        border-top: 1px solid #aaa;
        display: flex;
        justify-content: space-evenly;
        align-items: center;
        min-height: 50px;
        flex-wrap: wrap;

        & button {
            font-size: 16px;
            font-weight: 700;
        }
    }
}
</style>