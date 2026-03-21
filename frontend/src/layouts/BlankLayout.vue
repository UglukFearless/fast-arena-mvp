<template>
    <div class="blank-layout__content-wrapper">
        <slot name="main" />
    </div>
</template>

<style lang="scss">

body {
    background: #F2F5FF;
    margin: 0;
    padding: 0;
}
 
.blank-layout__content-wrapper {
    width: 100vw;
    height: 100vh;
    display: flex;
    justify-content: center;
}

</style>