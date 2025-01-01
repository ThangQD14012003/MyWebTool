<template>
  <div>
    <h3 class="text-center title-web">GENERATEdd QR CODE</h3>
    <img
      class="logo-content d-flex m-auto"
      src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSqVd5D1cECAaMN1UZCvv0wgTyS9c3586_YNQ&s"
      alt=""
    />
    <div class="container mt-4 mb-4">
      <div class="container-wrap d-flex">
        <form action="" @submit.prevent="StartGenQr">
          <!-- <h3>Options</h3> -->
          <div class="option">
            <div class="option1">
              <button
                type="button"
                @click="clickUrl"
                class="btn_option"
                :class="{ 'add-bg': isClickUrl }"
              >
                <i class="fa-solid fa-link"></i>
                <span>URL</span>
              </button>
            </div>
            <div class="option2">
              <button
                type="button"
                @click="clickText"
                class="btn_option"
                :class="{ 'add-bg': isClickText }"
              >
                <i class="fas fa-font"></i>
                <span>TEXT</span>
              </button>
            </div>
            <div class="optione">
              <button
                type="button"
                @click="clickSms"
                class="btn_option"
                :class="{ 'add-bg': isClickSms }"
              >
                <i class="fa-solid fa-comment-sms"></i>
                <span>SMS</span>
              </button>
            </div>
            <!-- <div class="option3">
                            <input @click="clickSms" type="button" class="btn_option" :class="{'add-bg': isClickSms}" value="SMS">
                        </div> -->
          </div>
          <div v-if="isClickUrl" class="input-data my-3">
            <label class="" for="">Website URL</label>
            <input
              @input="validate"
              class="form-control"
              :class="{ 'is-invalid': isErrorUrl }"
              type="text"
              placeholder="https://... || http://..."
              v-model="textData"
            />
            <span v-if="isErrorUrl" :class="{ 'invalid-feedback': isErrorUrl }"
              >URL must start with http or https</span
            >
          </div>
          <div v-if="isClickText" class="input-data my-3">
            <label class="" for="">Text</label>
            <input
              @input="validate"
              class="form-control"
              type="text"
              placeholder="abc..."
              v-model="textData"
            />
          </div>
          <div v-if="isClickSms" class="input-data my-3">
            <label class="" for="">Phone</label>
            <input
              @input="validate"
              class="form-control"
              :class="{ 'is-invalid': isErrorInputSms }"
              type="text"
              placeholder="0..."
              v-model="phoneNumber"
            />
            <span v-if="isErrorInputSms" :class="{ 'invalid-feedback': isErrorInputSms }"
              >Phone number must start with 0</span
            >
            <!-- <br v-if="isErrorInputSms"> -->
            <label class="" for="">SMS</label>
            <input
              @input="validate"
              class="form-control"
              type="text"
              placeholder="abc..."
              v-model="sms"
            />
          </div>
          <input
            type="submit"
            value="Get QR Code"
            class="btn btn-primary d-block mx-auto my-5"
          />
          <!-- <chrome-picker :value="color" @input="handleColorChange">
                    </chrome-picker> -->
          <div class="setting">
            <div class="settingbg setting-frame">
              <div @click="ClickSettingBgColor" class="setting-item-wrap">
                <i class="fa-solid fa-brush mx-4"></i>
                <span class="setting-content">Background color</span>
                <i class="fa-solid fa-plus icon-add"></i>
              </div>
              <div v-show="isClickSettingBgColor" class="choose-setting">
                <div
                  @click="ChooseColorBg('box-color-red')"
                  class="box-color box-color-red mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorBg('box-color-yellow')"
                  class="box-color box-color-yellow mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorBg('box-color-green')"
                  class="box-color box-color-green mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorBg('box-color-blue')"
                  class="box-color box-color-blue mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorBg('box-color-violet')"
                  class="box-color box-color-violet mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorBg('box-color-pink')"
                  class="box-color box-color-pink mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorBg('box-color-re2')"
                  class="box-color box-color-re2 mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorBg('box-color-yello2')"
                  class="box-color box-color-yello2 mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorBg('box-color-gree2')"
                  class="box-color box-color-gree2 mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorBg('box-color-blu2')"
                  class="box-color box-color-blu2 mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorBg('box-color-viole2')"
                  class="box-color box-color-viole2 mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorBg('box-color-pin2')"
                  class="box-color box-color-pin2 mx-4 mb-4"
                ></div>
                <div @click="CancelBgQr" class="delete-bg-qr d-flex mx-auto mb-3">
                  <button type="button" class="btn btn-info">Cancel</button>
                </div>
              </div>
            </div>
            <div class="settingbg setting-frame">
              <div @click="ClickSettingQrColor" class="setting-item-wrap">
                <i class="fa-solid fa-fill-drip mx-4"></i>
                <span class="setting-content">Qr color</span>
                <i class="fa-solid fa-plus icon-add"></i>
              </div>
              <div v-show="isClickSettingQrColor" class="choose-setting">
                <div
                  @click="ChooseColorQr('box-color-red')"
                  class="box-color box-color-red mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorQr('box-color-yellow')"
                  class="box-color box-color-yellow mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorQr('box-color-green')"
                  class="box-color box-color-green mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorQr('box-color-blue')"
                  class="box-color box-color-blue mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorQr('box-color-violet')"
                  class="box-color box-color-violet mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorQr('box-color-pink')"
                  class="box-color box-color-pink mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorQr('box-color-re2')"
                  class="box-color box-color-re2 mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorQr('box-color-yello2')"
                  class="box-color box-color-yello2 mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorQr('box-color-gree2')"
                  class="box-color box-color-gree2 mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorQr('box-color-blu2')"
                  class="box-color box-color-blu2 mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorQr('box-color-viole2')"
                  class="box-color box-color-viole2 mx-4 mb-4"
                ></div>
                <div
                  @click="ChooseColorQr('box-color-pin2')"
                  class="box-color box-color-pin2 mx-4 mb-4"
                ></div>
                <div @click="CancelColorQr" class="delete-bg-qr d-flex mx-auto mb-3">
                  <button type="button" class="btn btn-info">Cancel</button>
                </div>
              </div>
            </div>
            <div class="settingbg setting-frame">
              <div @click="ClickSettingLogo" class="setting-item-wrap">
                <i class="fa-regular fa-image mx-4"></i>
                <span class="setting-content">Logo</span>
                <i class="fa-solid fa-plus icon-add"></i>
              </div>
              <div v-if="isClickSettingLogo" class="choose-setting-logo">
                <div class="list-logo-setting">
                  <div
                    @click="
                      ClickChooseLogo(
                        'data:image/png;base64,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'
                      )
                    "
                    class="logo-setting-item"
                  >
                    <img
                      src="	https://qr.io/generator2/images/watermarks/01-link-b.png"
                      alt=""
                    />
                  </div>
                  <div
                    @click="
                      ClickChooseLogo(
                        'data:image/png;base64,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'
                      )
                    "
                    class="logo-setting-item"
                  >
                    <img
                      src="https://qr.io/generator2/images/watermarks/02-location.png"
                      alt=""
                    />
                  </div>
                  <div
                    @click="
                      ClickChooseLogo(
                        'data:image/png;base64,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'
                      )
                    "
                    class="logo-setting-item"
                  >
                    <img
                      src="	https://qr.io/generator2/images/watermarks/03-email.png"
                      alt=""
                    />
                  </div>
                  <div
                    @click="
                      ClickChooseLogo(
                        'data:image/png;base64,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'
                      )
                    "
                    class="logo-setting-item"
                  >
                    <img
                      src="https://qr.io/generator2/images/watermarks/04-whatsapp.png"
                      alt=""
                    />
                  </div>
                  <div
                    @click="
                      ClickChooseLogo(
                        'data:image/png;base64,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'
                      )
                    "
                    class="logo-setting-item"
                  >
                    <img
                      src="https://qr.io/generator2/images/watermarks/05-wifi.png"
                      alt=""
                    />
                  </div>
                </div>
                <input
                  class="input-logo-setting"
                  type="file"
                  @change="onLogoFileChange"
                  accept="image/*"
                />
              </div>
            </div>
          </div>
        </form>
        <div class="box-img">
          <div class="qr-title">
            <h4 class="text-center my-3">Your qr code</h4>
          </div>
          <div v-if="!qrCodeData" class="qrcode-content">
            <span class="text-center d-block">
              Click 'Get Qr Code' to get your qr code
            </span>
          </div>
          <img
            class="img-qr d-flex m-auto img-qr-dad"
            v-if="qrCodeData"
            :src="qrCodeData"
            alt=""
          />
          <!-- <img v-if="isClickChooseLogo" class="img-qr d-flex m-auto img-qr-child" src="https://qr.io/generator2/images/watermarks/01-link-b.png" alt=""> -->

          <input
            v-if="qrCodeData"
            type="button"
            @click="DownLoadQrCode"
            class="btn btn-primary d-flex mx-auto mt-4"
            value="DownLoad"
          />
        </div>
      </div>
      <form-message
        class="mt-5"
        v-if="isNullTextData"
        :content="`Fill full the information`"
        @cancel="close"
      />
    </div>
    <default-footer />
  </div>
</template>

<script>
import QRCode from "qrcode";
import FormMessage from "../../components/FormMesage/FormMessage.vue";
import DefaultFooter from "../../components/Footer/DefaultFooter.vue";
export default {
  head: {
    title: "IT TOOLS | GEN QR",
  },
  components: {
    FormMessage,
    DefaultFooter,
  },
  data() {
    return {
      textData: "",
      qrCodeData: null,
      sms: "",
      phoneNumber: "",
      colorBg: "",
      colorQr: "",
      isErrorInputSms: false,
      isErrorUrl: false,
      isClickSms: false,
      isClickUrl: true,
      isClickText: false,
      isNullTextData: false,

      isClickSettingBgColor: false,
      isClickSettingQrColor: false,
      isClickSettingLogo: false,
      isClickChooseLogo: true,
      // isClickGetQrCode: false,
    };
  },
  mounted() {
    this.setBoxColors();
  },
  methods: {
    close() {
      this.isNullTextData = false;
    },
    clickUrl() {
      this.colorBg = "";
      this.colorQr = "";
      this.qrCodeData = null;
      this.isErrorUrl = false;
      this.textData = "";
      this.isClickUrl = true;
      this.isClickText = false;
      this.isClickSms = false;
      this.qrCodeImage = null;
    },
    clickText() {
      this.colorBg = "";
      this.colorQr = "";
      this.qrCodeData = null;
      this.textData = "";
      this.isClickText = true;
      this.isClickUrl = false;
      this.isClickSms = false;
      this.qrCodeImage = null;
    },
    clickSms() {
      this.colorBg = "";
      this.colorQr = "";
      this.qrCodeData = null;
      this.phoneNumber = "";
      this.sms = "";
      this.isClickSms = true;
      this.isClickText = false;
      this.isClickUrl = false;
      this.qrCodeImage = null;
    },
    validate() {
      console.log("nhạp....");
      this.colorBg = "";
      this.colorQr = "";
      this.qrCodeData = null;
      if (
        this.isClickUrl &&
        (!this.textData.startsWith("http") || !this.textData.startsWith("https"))
      ) {
        this.isErrorUrl = true;
      }
      if (
        this.isClickUrl &&
        (this.textData.startsWith("http") || this.textData.startsWith("https"))
      ) {
        this.isErrorUrl = false;
      }
      // if (this.isClickSms && (!this.phoneNumber.startsWith("0"))) {
      //     this.isErrorInputSms = true;
      // }
      // if (this.isClickSms && this.phoneNumber.startsWith("0")) {
      //     this.isErrorInputSms = false;
      // }
    },
    DownLoadQrCode() {
      const link = document.createElement("a");
      link.href = this.qrCodeData;
      link.download = "qrcode.png";
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    },
    async StartGenQr() {
      if (!this.isClickSms && this.textData == "") {
        this.isNullTextData = true;
        return;
      }
      if (this.isClickSms && (this.sms == "" || this.phoneNumber == "")) {
        this.isNullTextData = true;
        return;
      }
      this.qrCodeData = null;
      console.log("this.qrCodeData: ", this.qrCodeData);
      console.log("gen qr code");
      if (!this.isClickSms) {
        console.log("no sms");
        try {
          const options = {
            errorCorrectionLevel: "H",
            width: 200,
            color: {
              dark: this.colorQr, // Màu của các khối mã QR code
              light: this.colorBg, // Màu nền của mã QR code
            },
          };
          this.qrCodeData = await QRCode.toDataURL(this.textData, options);
          console.log("qrcodedata: ", this.qrCodeData);
        } catch {
          console.log("fail");
        }
      } else {
        console.log("sms");
        try {
          const options = {
            errorCorrectionLevel: "H",
            width: 200,
            color: {
              dark: this.colorQr, // Màu của các khối mã QR code
              light: this.colorBg,
            },
          };
          const Sms = `SMSTO:${this.phoneNumber}:${this.sms}`;
          this.qrCodeData = await QRCode.toDataURL(Sms, options);
        } catch {
          console.log("fail");
        }
      }
    },
    setBoxColors() {
      const colorMap = {
        "box-color-red": "#ff0000",
        "box-color-yellow": "#ffff00",
        "box-color-green": "#00ff00",
        "box-color-blue": "#0000ff",
        "box-color-violet": "#8a2be2",
        "box-color-pink": "#ffc0cb",
        "box-color-re2": "#960000",
        "box-color-yello2": "#ffd700",
        "box-color-gree2": "#1a791a",
        "box-color-blu2": "#7777ff",
        "box-color-viole2": "#ce5cff",
        "box-color-pin2": "#ff69b4",
      };
      var i = 1;
      document.querySelectorAll(".box-color").forEach((box) => {
        const className = box.classList.value;
        console.log(`className ${i}: ${className}`);
        const keyColor = Object.keys(colorMap).find((key) => {
          return className.includes(key);
        });
        console.log(`keyColor ${i}: ${keyColor}`);
        if (keyColor) {
          box.style.backgroundColor = colorMap[keyColor];
        }
        i++;
      });
    },
    ClickSettingBgColor() {
      console.log("click");
      this.isClickSettingBgColor = !this.isClickSettingBgColor;
    },
    ClickSettingQrColor() {
      this.isClickSettingQrColor = !this.isClickSettingQrColor;
    },
    ClickSettingLogo() {
      this.isClickSettingLogo = !this.isClickSettingLogo;
    },
    ChooseColorBg(color) {
      this.isClickChooseBg = true;
      const colorMap = {
        "box-color-red": "#ff0000",
        "box-color-yellow": "#ffff00",
        "box-color-green": "#00ff00",
        "box-color-blue": "#0000ff",
        "box-color-violet": "#8a2be2",
        "box-color-pink": "#ffc0cb",
        "box-color-re2": "#960000",
        "box-color-yello2": "#ffd700",
        "box-color-gree2": "#1a791a",
        "box-color-blu2": "#7777ff",
        "box-color-viole2": "#ce5cff",
        "box-color-pin2": "#ff69b4",
      };
      this.colorBg = colorMap[color];
      this.StartGenQr();
    },
    ChooseColorQr(color) {
      const colorMap = {
        "box-color-red": "#ff0000",
        "box-color-yellow": "#ffff00",
        "box-color-green": "#00ff00",
        "box-color-blue": "#0000ff",
        "box-color-violet": "#8a2be2",
        "box-color-pink": "#ffc0cb",
        "box-color-re2": "#960000",
        "box-color-yello2": "#ffd700",
        "box-color-gree2": "#1a791a",
        "box-color-blu2": "#7777ff",
        "box-color-viole2": "#ce5cff",
        "box-color-pin2": "#ff69b4",
      };
      this.colorQr = colorMap[color];
      this.StartGenQr();
    },
    async generateQRCodeWithLogo() {
      if (this.logo) {
        console.log("logo hàm gen: ", this.logo);
        const canvas = document.createElement("canvas");
        const ctx = canvas.getContext("2d");
        const imgQRCode = new Image();
        imgQRCode.src = this.qrCodeData;

        imgQRCode.onload = () => {
          const logoImg = new Image();
          logoImg.crossOrigin = "Anonymous";
          logoImg.src = this.logo;

          logoImg.onload = () => {
            canvas.width = imgQRCode.width;
            canvas.height = imgQRCode.height;
            ctx.drawImage(imgQRCode, 0, 0);

            // Tính kích thước và vị trí logo
            const logoSize = Math.floor(canvas.width / 5);
            const x = (canvas.width - logoSize) / 2;
            const y = (canvas.height - logoSize) / 2;
            ctx.drawImage(logoImg, x, y, logoSize, logoSize);
            this.qrCodeData = canvas.toDataURL("image/png", 1.0);
          };
        };
      }
    },
    onLogoFileChange(event) {
      const file = event.target.files[0];
      const reader = new FileReader();
      reader.onload = (e) => {
        this.logo = e.target.result;
        // console.log('e.target.value: ', e.target.result);
        this.generateQRCodeWithLogo();
      };
      reader.readAsDataURL(file);
      //readAsDataURL là phương thức của FileReader để đọc tệp và
      //chuyển đổi nó thành URL dữ liệu. Điều này cho phép hiển thị tệp hình ảnh hoặc các loại
      //tệp khác trong ứng dụng web mà không cần phải tải tệp lên máy chủ trước.
    },
    async ClickChooseLogo(src) {
      await this.StartGenQr();
      var a = src;
      this.logo = a;
      this.generateQRCodeWithLogo();
    },
    async CancelBgQr() {
      this.colorBg = "";
      await this.StartGenQr();
    },
    async CancelColorQr() {
      this.colorQr = "";
      await this.StartGenQr();
    },
  },
};
</script>

<style scoped>
.container {
  padding: 20px 60px 60px;
  width: 1000px;
  background-color: #f3f7fa;
  border-radius: 6px;
  height: 100%;
}

.logo-content {
  height: 100px;
  width: auto;
  /* padding: 20px; */
  margin-bottom: 20px;
}

.option {
  display: flex;
  justify-content: space-evenly;
}

.btn_option {
  background-color: rgb(198, 239, 253);
  min-width: 80px;
  outline: none;
  border: none;
  border-radius: 6px;
  padding: 6px 10px;
  /* color: #f3f7fa; */
}

.qrcode-content {
  margin-top: 40px;
}

.add-bg {
  background-color: rgb(71, 202, 246);
  /* color: rgb(98, 167, 144); */
  transition: 0.4s;
}

.box-img {
  border: 2px dashed grey;
  width: 300px;
  height: 400px;
  position: sticky;
  /* right: 300px; */
  top: 20px;
}

form {
  flex: 1;
  margin-right: 20px;
}

.img-qr {
  /* width: 200px; */
  color: red;
}

/* .img-qr-dad {
    position: relative;
} */
.img-qr-child {
  position: relative;
  width: 40px;
  height: auto;
  top: -120px;
}

.setting-frame {
  background-color: #ffffff;
  height: 100%;
  border-top-left-radius: 6px;
  border-top-right-radius: 6px;
  border: 2px solid rgba(0, 0, 0, 0.1);

  margin-top: 30px;
}

.setting-item-wrap:hover {
  color: rgb(64, 153, 255);
  cursor: pointer;
}

.setting-content {
  line-height: 70px;
  /* background-color: red; */
}

.icon-add {
  position: relative;
  top: -40px;
  right: 18px;
  display: flex;
  justify-content: flex-end;
}

.box-color {
  width: 40px;
  height: 40px;
  cursor: pointer;
  border-radius: 6px;
}

.choose-setting {
  display: flex;
  flex-wrap: wrap;
}

.list-logo-setting {
  display: flex;
  justify-content: space-around;
  flex-wrap: wrap;
}

.logo-setting-item img {
  width: 60px;
  height: auto;
  margin-bottom: 20px;
  padding: 10px;
  border-radius: 6px;
}

.logo-setting-item img:hover {
  background-color: rgb(172, 206, 245);
  cursor: pointer;
  border: 1px solid rgba(0, 0, 0, 0.1);
  /* transition: 0.6s; */
  outline: none;
}

.input-logo-setting {
  margin: 10px;
}
</style>
