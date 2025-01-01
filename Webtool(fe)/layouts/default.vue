<template>
<div class="my-body">
    <default-header />
    <nuxt class="main">
        
    </nuxt>
    <!-- <default-footer class="footer" /> -->
</div>

<!-- <div class="wrap">
        <div class="wrap1">

        </div>
    </div> -->
</template>

<script>
import DefaultHeader from '../components/Header/DefaultHeader.vue'
import DefaultFooter from '../components/Footer/DefaultFooter.vue';
export default {
    components: {
        DefaultHeader,
        DefaultFooter
    }
}
</script>

<style scoped>
.my-body {
    height: 100%;
    position: relative;
    /* display: flex;
    flex-direction: column;  */
    min-height: 100vh;
    background-color: #ffffff;
    /* background-image: url(https://media.istockphoto.com/id/1358004763/vector/abstract-light-blue-and-white-background-poster-with-dynamic-technology-network-vector.jpg?s=612x612&w=0&k=20&c=oRRHmXSYdJsxTr34nBOvATW4XiB6u8Jws56_YE9MZ6k=);
    background-size: 100%;
    background-repeat: no-repeat; */
}

.footer {
    position: absolute;
    bottom: 0;
}

.main {
    padding-top: 80px;
    background-color: #f3f7fa;
    /* height: 1000px; */
}
</style>
