<template>
    <FightLayout>
        <template v-slot:main>
            <FightPanel />
        </template>
    </FightLayout>
</template>

<script setup lang="ts">
import FightLayout from '@/layouts/FightLayout.vue';
import FightPanel from '@/components/Fight/FightPanel.vue';

</script>