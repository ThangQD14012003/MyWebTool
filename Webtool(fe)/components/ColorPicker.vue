<template>
  <div>
    <div class="color-picker-panel">
      <input type="text" v-model="color.hex" class="form-control input-color" />
      <div class="color-display-wrap">
        <client-only v-if="showColorPicker">
          <chrome-picker :value="color.hex" @input="updateColor" :disableAlpha="true" />
        </client-only>
        <div class="color-collect">
          <div
            @click="toggleColorPicker"
            class="color-display"
            :style="{ backgroundColor: color.hex }"
          ></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { Chrome as ChromePicker } from "vue-color";
export default {
  components: {
    ChromePicker,
  },
  props: {
    // color: {
    //     hex: {
    //         Type: String,
    //         default: ''
    //     }
    // }
  },
  data() {
    return {
      showColorPicker: false,
      color: {
        hex: "#000000",
      },
    };
  },
  methods: {
    toggleColorPicker() {
      this.showColorPicker = !this.showColorPicker;
    },
    updateColor(color) {
      if (color && color.hex) {
        this.color = color;
      } else {
        console.error("Invalid color object:", color);
      }
      this.$emit("chooseColor", this.color.hex);
    },
  },
};
</script>

<style scoped>
.color-picker-panel {
  display: flex;
  align-items: center;
  flex-direction: row;
}

.color-display-wrap {
  padding: 2px 2px;
}

.color-display {
  width: 140px;
  height: 42px;
  cursor: pointer;
  margin: 4px 10px;
}
.color-collect {
  /* border: 1px solid rgb(200, 200, 200) */
}
.input-color {
  flex: 1;
  width: 450px;
  margin-left: 20px;
  margin-right: 10px;
}
</style>
