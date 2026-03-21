<template>
    <BaseLayout>
        <template v-slot:main>
            <AvailableActivitiesPanel />
        </template>
    </BaseLayout>
</template>

<script setup lang="ts">
import BaseLayout from '@/layouts/BaseLayout.vue';
import AvailableActivitiesPanel from '@/components/Arena/AvailableActivitiesPanel.vue';

</script>