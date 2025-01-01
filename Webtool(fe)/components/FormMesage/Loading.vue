<template>
<div>
    <div class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <!-- <div class="my-3 text-center">
                    <span>Checking {{numberRow}} / {{ endRow - startRow + 1}}</span>
                </div> -->
                <div class="text-center">
                    <span class="text-center">{{ title }} {{ progressPercentage }} %</span>
                </div>
                
                <div class="progress" style="height: 20px;">
                    <div class="progress-bar" rolbare="progressbar" :style="{width: progressPercentage + '%',transition: 'width 0.000000000000000000001s'}" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
                </div>
            </div>
        </div>
    </div>
</div>
</template>

<script>
import { Number } from 'core-js';

export default {
    props: {
        numberRow: {
            type: Number,
            default: 0,
        },
        startRow: {
            type: Number,
            default: 0,
        },
        endRow: {
            type: Number,
            default: 0,
        },
        progressPercentage: {
            type: Number,
            default: 0,
        },
        title: {
            type: String, 
            default: '', 
        }
      
    },
}
</script>

<style scoped>
.modal {
    position: fixed;
    /* top: auto;
    right: auto;
    bottom: auto;
    left: auto; */
    z-index: 1;
    display: block;
    background-color: rgba(0, 0, 0, .4);
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
}

.progress {
    display: flex;
    height: 1rem;
    overflow: hidden;
    font-size: .75rem;
    background-color: #e9ecef;
    border-radius: .25rem;
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
    transition: width .6s ease;
}
</style>
