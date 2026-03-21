<template>
    <BlankLayout>
        <template v-slot:main>
            <RegistrationForm />
        </template>
    </BlankLayout>
</template>

<script setup lang="ts">
import BlankLayout from '../layouts/BlankLayout.vue';
import RegistrationForm from '../components/Registration/RegistrationForm.vue';
</script>