<template>
    <BaseLayout>
        <template v-slot:main>
            <HeroInfo />
        </template>
    </BaseLayout>
</template>

<script setup lang="ts">
import BaseLayout from '@/layouts/BaseLayout.vue';
import HeroInfo from '@/components/HeroInfo/HeroInfo.vue';
</script>