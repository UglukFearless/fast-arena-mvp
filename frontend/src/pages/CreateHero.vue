<template>
    <BaseLayout>
        <template v-slot:main>
            <section class="create-hero__content">
                <p class="create-hero__title-text">
                    Заполните форму создания героя
                </p>
                <CreateHeroForm />
            </section>
        </template>
    </BaseLayout>
</template>

<script setup lang="ts">
import BaseLayout from '@/layouts/BaseLayout.vue';
import CreateHeroForm from '@/components/CreateHero/CreateHeroForm.vue';
</script>

<style lang="scss">
.create-hero {
    
    &__content {
        background: white;
        margin: 12px auto;
        padding: 12px;
        box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);
        border-radius: 8px;
        max-width: 480px;
        width: 100vw;
        display: flex;
        justify-content: center;
        flex-direction: column;
        font-size: 18px;
    }

    &__title-text {
        text-align: center;
        font-weight: bold;
        padding: 8px 0;
    }
}
</style>