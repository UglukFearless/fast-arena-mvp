<template>
    <section class="available-activities">
        <div class="available-activities__content">
            <div class="available-activities__content-wrapper">
                <AvailiableActivity v-for="activity in availiableActivities" :activity="activity" />
            </div>
        </div>  
    </section>
</template>

<script setup lang="ts">
import { computed } from 'vue';

import { useActivityStore } from '@/stores/activity';
import AvailiableActivity from '@/components/Arena/AvailiableActivity.vue';

const activityStore = useActivityStore();

const availiableActivities = computed(() => activityStore.activities);

activityStore.init();

</script>

<style lang="scss">
.available-activities {
    border-bottom: 1px solid #aaa;
    

    &__content {
        width: 1280px;
        margin: 0 auto;
        max-width: 100%;
    }

    &__content-wrapper {
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
        margin: 12px 0;
    }
}
</style>