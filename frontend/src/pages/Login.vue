<template>
    <BlankLayout>
        <template v-slot:main>
            <LoginForm />
        </template>
    </BlankLayout>
</template>

<script setup lang="ts">
import BlankLayout from '../layouts/BlankLayout.vue';
import LoginForm from '../components/Login/LoginForm.vue';
</script>