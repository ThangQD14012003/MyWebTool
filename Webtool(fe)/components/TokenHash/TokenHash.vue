<template>
  <div class="form-group hash">
    <div class="hash-title-dad my-6">
      <h3 class="text-center">API Tokens with Base 64, MD5 Hash, Sha256 and Sha512</h3>
    </div>
    <div class="hash-content-dad">
      <hr />
      <div class="base64 hash-content-dad-item">
        <div class="base64-title hash-title">Base64</div>
        <div class="base64-content hash-content">
          {{ base64 }}
        </div>
      </div>
      <hr />
      <div class="hash-content-dad-item">
        <div class="hash-title">MD5 Hash</div>
        <div class="hash-content">
          {{ md5Hash }}
        </div>
      </div>
      <hr />
      <div class="hash-content-dad-item">
        <div class="hash-title">Sha256</div>
        <div class="hash-content">
          {{ sha256 }}
        </div>
      </div>
      <hr />
      <div class="hash-content-dad-item">
        <div class="hash-title">Sha512</div>
        <div class="hash-content">
          {{ sha512 }}
        </div>
      </div>
      <hr />
    </div>
  </div>
</template>

<script>
export default {
  props: {
    base64: "",
    md5Hash: "",
    sha256: "",
    sha512: "",
  },
  data() {
    return {};
  },
};
</script>
<style scoped>
.hash-content-dad-item {
  display: flex;
}

.hash-title {
  min-width: 120px;
  /* margin-right: 40px;  */
}

.hash-content {
  width: 100%;
  height: auto;
  word-break: break-all;
}
</style>
