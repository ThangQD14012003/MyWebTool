<template>
  <div>
    <h3 class="text-center my-4">IMPORT EXCEL TO DATABASE</h3>
    <div class="Image text-center">
      <img
        class="excel"
        src="https://cellphones.com.vn/sforum/wp-content/uploads/2021/12/chen-anh-vao-excel.jpeg"
        alt=""
      />
      <img
        class="arrow"
        src="https://png.pngtree.com/png-clipart/20230315/original/pngtree-right-arrow-icon-png-image_8989371.png"
        alt=""
      />
      <img
        class="sql"
        src="https://mesidas.com/wp-content/uploads/2020/11/SQL-la-gi.png"
        alt=""
      />
    </div>
    <div class="containerr">
      <form-message
        v-if="errorCo == 1"
        @cancel="close"
        content="Connection String không hợp lệ"
      />
      <h2 class="text-center">Điền vào các dữ liệu cần thiết</h2>
      <div class="form-group">
        <label for="connectionStrings">Connection String:</label>
        <input
          class="form-control"
          v-model="connectionStrings"
          id="connectionStrings"
          required
        />
      </div>
      <button @click="getTable" class="btn btn-primary">Next</button>

      <br />
      <div class="my-3" v-if="table.length">
        <select class="form-control" v-model="tableName">
          <option disabled value="">Chọn table cần import dữ liệu</option>
          <option v-for="(tableItem, index) in table" :key="index" :value="tableItem">
            {{ tableItem }}
          </option>
        </select>
      </div>
      <form-message
        v-if="errorTa == 1"
        @cancel="close"
        content="Chọn tên table để tiếp tục..."
      />
      <!-- <button v-if="table.length" class="btn btn-primary" @click="getColumnsAuto">Auto-fill primary key</button>
        <button v-if="table.length" class="btn btn-primary" @click="getColumns">Fill primary key</button> -->
      <div v-if="table.length && tableName != ''">
        <div>
          <input
            class="btn btn-primary"
            value="Get Colums"
            style=""
            type="button"
            id="getColumn1"
            name="getColumn"
            @click="getColumns"
          />
          <!-- <label for="getColumn1">Get Columns</label> -->
        </div>
      </div>

      <div class="mapping">
        <div v-if="dbColumns.length">
          <form @submit.prevent="submitMapping">
            <h3 class="text-center">
              Điền các cột trong file excel tương ứng với các trường trong database
            </h3>
            <div v-for="(column, index) in dbColumns" :key="index">
              <div>
                <div v-if="column.isIdentity" class="form-group">
                  <label :for="'dbColumn_' + index"
                    >{{ column.columnName }} ({{ column.dataType }}, Is_Nullable:
                    {{ column.isNullable }})
                  </label>
                  <input
                    disabled
                    placeholder="Khóa chính tự tăng"
                    class="form-control"
                    v-model="columnMappings[column]"
                    :id="'dbColumn_' + index"
                  />
                </div>
                <div v-else class="form-group">
                  <label :for="'dbColumn_' + index"
                    >{{ column.columnName }} ({{ column.dataType }}, Is_Nullable:
                    {{ column.isNullable }})</label
                  >
                  <input
                    class="form-control"
                    v-model="columnMappings[column.columnName]"
                    :id="'dbColumn_' + index"
                  />
                </div>
              </div>
            </div>
            <h3 class="text-center">Khoảng dữ liệu import</h3>
            <div>
              <div class="form-group">
                <label for="">Start Row</label>
                <input type="number" class="form-control" v-model="startRow" required />
              </div>
              <div class="form-group">
                <label for="">End Row</label>
                <input type="number" class="form-control" v-model="endRow" required />
              </div>
              <div class="form-group">
                <label for="">Sheet Name</label>
                <input class="form-control" v-model="sheetName" required />
              </div>
            </div>
            <input style="margin-bottom: 20px" type="file" ref="myFile" required />

            <!-- <div class="progress" v-if="loading">
                        <progress class="progress-bar" :value="progressPercentage" max="100"></progress>
                        <p>{{ progressPercentage }}%</p>
                    </div> -->
            <br />
            <button class="btn btn-success my-1" type="submit">Submit</button>
            <form-message
              v-if="errorMap == 1"
              @cancel="close"
              :content="`Cần chọn trường để import...`"
            />
            <form-message-3
              v-if="notification == 1"
              :content="`Số dòng lỗi: ${this.errorRows}, số dòng không lỗi: ${this.successRows}. Tiếp tục import?`"
              @continueMap="okContinue"
              @cancelMap="closeMap"
              @view="viewError"
            />
            <form-message
              v-if="importSuccess == 1"
              :content="`Đã import ${this.successRows} dòng dữ liệu`"
              @cancel="close"
            />
            <form-message
              v-if="errorSubmitRow == true"
              content="Kiểm tra lại các row import hoặc column import"
              @cancel="close"
            />
            <form-message
              v-if="viewErrorDetail"
              :content="`Dòng lỗi: ${this.errorRowsDetail}`"
              @cancel="closeviewErrorDetail"
              class="error-message"
            />
            <form-message
              v-if="isStartBiggerEnd"
              :content="`End row phải >= Start row`"
              @cancel="close"
              class="error-message"
            />
            <form-message
              v-if="isNotSheet"
              :content="`Sheet không tồn tại`"
              @cancel="close"
              class="error-message"
            />

            <div>
              <loading
                v-if="loading && notification == 0"
                :numberRow="numberRow"
                :startRow="parseInt(startRow)"
                :endRow="parseInt(endRow)"
                :progressPercentage="progressPercentage"
                :title="`Checking...`"
              />
            </div>
            <div>
              <loading
                v-if="loading && notification == 1"
                :numberRow="numberRow"
                :startRow="1"
                :endRow="successRows"
                :progressPercentage="progressPercentage"
                :title="`Importing...`"
              />
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import * as signalR from "@microsoft/signalr";
import FormMessage from "../../components/FormMesage/FormMessage.vue";
import FormMessage2 from "../../components/FormMesage/FormMessage2.vue";
import FormMessage3 from "../../components/FormMesage/FormMessage3.vue";
import Loading from "../../components/FormMesage/Loading.vue";

export default {
  components: {
    FormMessage,
    FormMessage2,
    FormMessage3,
    Loading,
  },
  data() {
    return {
      connectionStrings: "",
      tableName: "",
      dbColumns: [],
      columnMappings: {},
      startRow: 0,
      //startRow: null bị lỗi
      endRow: 0,
      sheetName: "DM1",
      table: [],
      message: [],
      errorCo: 0,
      errorTa: 0,
      errorMap: 0,
      notification: 0,
      errorRows: 0,
      successRows: 0,
      importSuccess: 0,
      errorSubmitRow: false,
      viewErrorDetail: false,
      errorRowsDetail: [0],

      loading: false,
      progressPercentage: 0,
      hubConnection: null,

      isStartBiggerEnd: false,

      numberRow: 0,
      Sheet: "",
      isNotSheet: false,
    };
  },
  methods: {
    close() {
      this.errorCo = 0;
      this.errorTa = 0;
      this.errorMap = 0;
      this.importSuccess = 0;
      this.notification = 0;
      this.errorSubmitRow = false;
      this.isStartBiggerEnd = false;
      this.isNotSheet = false;
    },
    closeviewErrorDetail() {
      this.viewErrorDetail = false;
    },
    closeMap() {
      this.notification = 0;
    },
    okContinue() {
      this.notification = 1;
      this.submitMapping();
    },
    viewError() {
      this.viewErrorDetail = true;
    },
    async getTable() {
      try {
        this.tableName = "";
        this.dbColumns = [];
        const res = await this.$axios.$post(`${process.env.baseApiUrl}/get-table`, {
          connectionStrings: this.connectionStrings,
          tableName: this.tableName,
          startRow: this.startRow,
          endRow: this.endRow,
          sheetName: this.sheetName,
        });
        console.log(res);
        console.log(typeof res);
        this.table = res;
      } catch {
        this.errorCo = 1;
      }
    },
    async getColumns() {
      try {
        if (this.tableName === "") {
          this.errorTa = 1;
        } else {
          const response = await this.$axios.$post(
            `${process.env.baseApiUrl}/get-columnsinfo`,
            {
              connectionStrings: this.connectionStrings,
              tableName: this.tableName,
              startRow: this.startRow,
              endRow: this.endRow,
              sheetName: this.sheetName,
            }
          );
          this.dbColumns = response;
          console.log("dbcolumn: ", this.dbColumns);
          console.log("first: ", this.dbColumns[0].columnName);
          this.columnMappings = this.dbColumns.reduce((acc, col) => {
            acc[col.columnName] = "";
            return acc;
          }, {});
        }
        console.log(this.columnMappings);
      } catch (ex) {
        console.log("Lỗi");
      }
    },
    async submitMapping() {
      console.log("this.columnmapping: ", this.columnMappings);
      const startRow = parseInt(this.startRow);
      const endRow = parseInt(this.endRow);
      if (startRow > endRow) {
        console.log(this.startRow);
        console.log(this.startRow);
        // console.log("đã lớn hơn")
        this.isStartBiggerEnd = true;
        return;
      }

      this.loading = true;
      let expectedRow = 1;

      const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7146/Hub-SignalR")
        .build();
      connection.on("ReceiveProgress", (myMessage) => {
        const receiveRow = Number(myMessage);
        if (receiveRow === expectedRow) {
          this.numberRow = receiveRow;
          this.progressPercentage = Math.floor(
            (this.numberRow / (this.endRow - this.startRow + 1)) * 100
          );
          console.log("numberRow", this.numberRow);
          expectedRow++;
        }
      });
      connection.start();
      try {
        const formData = new FormData();
        formData.append("file", this.$refs.myFile.files[0]);
        formData.append("connectionStrings", this.connectionStrings);
        formData.append("tableName", this.tableName);
        formData.append("columnMappings", JSON.stringify(this.columnMappings));
        formData.append("startRow", this.startRow);
        formData.append("endRow", this.endRow);
        formData.append("sheetName", this.sheetName);
        console.log("start");

        if (this.notification === 0) {
          const res = await this.$axios.$post(`${process.env.baseApiUrl}`, formData, {
            headers: {
              "Content-Type": "multipart/form-data",
            },
          });

          this.errorRowsDetail = res.errorRows;
          this.Sheet = res.errorSheet;
          console.log(this.Sheet);
          if (this.Sheet == "No") {
            this.isNotSheet = true;
            return;
          }
          console.log(this.errorRowsDetail);
          this.errorRows = res.errorRows.length;
          this.successRows = res.successRows.length;
          this.notification = 1;
          return;
        }
        if (this.notification === 1) {
          const res = await this.$axios.$post(
            `${process.env.baseApiUrl}/NoError`,
            formData,
            {
              headers: {
                "Content-type": "multipart/form-data",
              },
            }
          );
          this.successRows = res.successRows.length;
          this.importSuccess = 1;
          const importData = {
            connectionStrings: this.connectionStrings,
            tableName: this.tableName,
            columnMappings: JSON.stringify(this.columnMappings),
            startRow: this.startRow,
            endRow: this.endRow,
            sheetName: this.sheetName,
          };
          console.log("importData ", importData);
        }
      } catch (ex) {
        this.errorSubmitRow = true;
      } finally {
        this.loading = false;
        this.progressPercentage = 0;
      }
    },
  },
};
</script>

<style scoped>
.containerr {
  padding: 60px;
  width: 800px;
  margin: auto;
}

.mapping {
  margin-top: 30px;
}

.arrow {
  height: 100px;
  margin-left: 20px;
}

.excel {
  height: 200px;
  width: auto;
}

.sql {
  width: 300px;
}

.Image {
  margin-top: 30px;
}

.progress-bar {
  display: flex;
  flex-direction: column;
  justify-content: center;
  overflow: hidden;
  color: #fff;
  text-align: center;
  white-space: nowrap;
  background-color: #0d6efd;
  transition: width 0.6s ease;
}

.error-message {
  white-space: pre-wrap;
  /* Giữ nguyên các khoảng trắng và ngắt dòng theo nội dung */
  word-wrap: break-word;
  /* Ngắt từ nếu quá dài */
  max-width: 100%;
  /* Đảm bảo phần tử không vượt quá chiều rộng của phần chứa */
}
</style>
