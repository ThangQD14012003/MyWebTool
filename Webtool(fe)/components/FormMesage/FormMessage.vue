<template>
<div>
    <div class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Error</h5>
                    <button @click="onCloseModal" type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p class="content-body scrollable-content ">{{ content }}</p>
                </div>
                <div class="modal-footer">
                    <button @click="onCloseModal" type="button" class="btn btn-primary">Ok</button>
                    <!-- <button type="button" class="btn btn-" data-dismiss="modal">Cancel</button> -->
                </div>
            </div>
        </div>
    </div>
</div>
</template>

<script>
export default {
    props: {
        content: {
            type: String,
            default: '',
        }
    },
    methods: {
        onCloseModal() {
            this.$emit("cancel")
        }
    }
}
</script>

<style scoped>
.modal {
    position: fixed;
    z-index: 1;
    display: block;
    background-color: rgba(0, 0, 0, .4);
    transition: opacity 0.6s ease, transform 0.6s ease;
    /* opacity: 0.4; */
    /* transform: translateY(-50px); */
    /* Bắt đầu từ vị trí trên cùng */
    /* visibility: hidden; */
    /* Ẩn modal */
}

.modal-dialog {
    left: auto;
    margin-right: auto;
    margin-left: auto;
}

.modal-content {
    position: relative;
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    -webkit-box-orient: vertical;
    -webkit-box-direction: normal;
    -ms-flex-direction: column;
    flex-direction: column;
    width: 100%;
    pointer-events: auto;
    background-color: #fff;
    background-clip: padding-box;
    border: 1px solid rgba(0, 0, 0, .2);
    border-radius: .3rem;
    outline: 0;
    margin-top: 85px; 
}

.modal-header {
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    -webkit-box-align: start;
    -ms-flex-align: start;
    align-items: flex-start;
    -webkit-box-pack: justify;
    -ms-flex-pack: justify;
    justify-content: space-between;
    padding: 1rem;
    border-bottom: 1px solid #e9ecef;
    border-top-left-radius: .3rem;
    border-top-right-radius: .3rem;
}

.modal-body {
    position: relative;
    -webkit-box-flex: 1;
    -ms-flex: 1 1 auto;
    flex: 1 1 auto;
    padding: 1rem;
}

.modal-footer {
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    -webkit-box-align: center;
    -ms-flex-align: center;
    align-items: center;
    -webkit-box-pack: end;
    -ms-flex-pack: end;
    justify-content: flex-end;
    padding: 1rem;
    border-top: 1px solid #e9ecef;

}

.scrollable-content {
    max-height: 300px;
    overflow-y: auto;
    /* Thêm thanh cuộn dọc khi nội dung vượt quá chiều cao tối đa */
    border: 1px solid #ccc;
    padding: 10px;
    background-color: #f9f9f9;
}
</style>
