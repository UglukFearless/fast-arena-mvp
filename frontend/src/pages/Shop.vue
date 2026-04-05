<template>
    <BaseLayout>
        <template v-slot:main>
            <ShopPage />
        </template>
    </BaseLayout>
</template>

<script setup lang="ts">
import BaseLayout from '@/layouts/BaseLayout.vue';
import ShopPage from '@/components/Shop/ShopPage.vue';
</script>
