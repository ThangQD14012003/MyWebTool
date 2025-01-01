<template>
  <div>
    <header>
      <!-- <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQdIQNtM1KoFuSyzeXiwcCX8ZHPvufDU1WcHQ&s" alt=""> -->
      <nav>
        <div class="logo">
          <div class="title-logo">
            <span>IT-TOOLS</span>
          </div>
          <img
            class="img-logo ms-5 d-block"
            src="https://pbs.twimg.com/profile_images/1518273966837796872/rda9RXuj_400x400.jpg"
            alt=""
          />
        </div>
        <ul>
          <li :class="{ active: activeItem === '/' }" @click="setActive('/')">
            <nuxt-link exact to="/"><span class="a">IMPORT EXCEL</span></nuxt-link>
          </li>
          <li
            :class="{ active: activeItem === '/gen_token' }"
            @click="setActive('/gen_token')"
          >
            <nuxt-link to="/gen_token"><span class="a">GEN TOKEN</span></nuxt-link>
          </li>
          <li
            :class="{ active: activeItem === '/gen_qrcode' }"
            @click="setActive('/gen_qrcode')"
          >
            <nuxt-link to="/gen_qrcode"><span class="a">GEN QR CODE</span></nuxt-link>
          </li>
          <!-- <li
            :class="{ active: activeItem === '/gen_qrcode_2' }"
            @click="setActive('/gen_qrcode_2')"
          >
            <nuxt-link to="/gen_qrcode_2"><span class="a">GEN QR CODE 2</span></nuxt-link>
          </li> -->
        </ul>
      </nav>
    </header>
  </div>
</template>

<script>
// import { useRouter, useRoute } from 'vue-router';
export default {
  data() {
    return {
      isClick: false,
      activeItem: "",
      // route: useRoute()
    };
  },
  created() {
    this.activeItem = this.$route.path;
  },
  methods: {
    setActive(string) {
      this.isClick = true;
      this.activeItem = string;
      this.$router.push(string);
    },
  },
};
</script>

<style scoped>
header {
  background-color: #000000;
  padding: 14px;
  box-shadow: 0px 4px 8px rgba(93, 93, 93, 0.5);
  position: fixed;
  top: 0;
  width: 100%;
  z-index: 2;
}

.logo {
  position: relative;
}

.img-logo {
  position: absolute;
  top: 0;
  right: -60px;
}

.title-logo {
  /* position: absolute; */
  margin-left: 10px;
  margin-top: 10px;
  color: rgb(64, 153, 255);
  font-weight: 500;
}

img {
  position: relative;
  z-index: 10;
  width: 40px;

  top: 28px;
}

nav {
  position: relative;
  z-index: 1;
  display: flex;
}

ul {
  /* margin: 0; */
  display: flex;
  /* justify-content: center;
    align-items: center; */
  margin: auto;
  padding: 0;
  /* justify-content: flex-end;
    margin-right: 100px; */
}

a {
  text-decoration: none;
  color: rgb(255, 255, 255);
  /* width: 80px; */
  /* display: inline-block; */
  display: flex;
  margin: auto;
}

li:hover a {
  color: rgb(64, 153, 255);
}

li {
  cursor: pointer;
  padding: 8px 10px;
  border-radius: 20px;
  width: 160px;
  margin-left: 14px;
}

/* 
.li-click {
    background-color: rgb(255, 246, 246);
} */
li.active {
  background-color: rgb(246, 246, 246);
}

li:hover {
  background-color: rgb(246, 246, 246);
  transition: 0.3s;
}

a.nuxt-link-active {
  color: rgb(64, 153, 255);
}

span {
  display: block;
  /* display: flex; */
  margin: auto;
}

.a {
  font-weight: 500;
}
</style>
