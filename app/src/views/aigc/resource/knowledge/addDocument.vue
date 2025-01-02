<template>
    <el-dialog title="添加文档" v-model="visible" :width="800" destroy-on-close @closed="$emit('closed')">
        <el-main>
            <el-steps :active="active" align-center style="margin-bottom: 20px;">
                <el-step title="选中数据源"></el-step>
                <el-step title="文本分段与清洗"></el-step>
                <el-step title="处理并完成"></el-step>
            </el-steps>
            <el-row>
                <el-col>
                    <el-form v-if="active==0" ref="stepForm_0" :model="form" :rules="rules" label-position="top">
                        <el-form-item label="文件" prop="file4">
                            <sc-upload-file v-model="form.documentId" :limit="1" drag :apiObj="uploadApi" accept="image/jpg,image/png">
                                <el-icon class="el-icon--upload"><el-icon-upload-filled /></el-icon>
                                <div class="el-upload__text">
                                    点击上传或拖拽文档到这里<br />
                                    支持 PDF、TXT、DOC、DOCX、MD，最多可上传 300 个文件，每个文件不超过 100MB， PDF 最多 500 页
                                </div>
                            </sc-upload-file>
                        </el-form-item>

                    </el-form>
                    <el-form v-if="active==1" ref="stepForm_1" :model="form" :rules="rules" label-position="top">
                        <el-card shadow="never" header="自动分段与清洗" :class="splitType0Class" @click="()=>this.form.splitType=0">
                            自动分段与预处理规则
                        </el-card>
                        <el-card shadow="never" header="自定义" :class="splitType1Class" @click="()=>this.form.splitType=1">
                            自定义分段规则、分段长度及预处理规则
                            <el-form ref="form" :model="form" label-width="120px" style="margin-top:20px;" v-show="form.splitType==1">
                                <el-form-item label="分段标识符">
                                    <el-select v-model="form.splitOptions" multiple style="width: 100%">
                                        <el-option v-for="item in splitOptions" :key="item.value" :label="item.label" :value="item.value" />
                                    </el-select>
                                </el-form-item>
                                <el-form-item label="分段最大长度">
                                    <el-input-number v-model="form.maxTokensPerLine" :min="100" :max="5000" style="width: 100%" />
                                </el-form-item>
                                <el-form-item label="分段重叠度%">
                                    <el-input-number v-model="form.overlap" :min="0" :max="90" style="width: 100%" />
                                </el-form-item>
                                <el-form-item label="文本预处理规则">
                                    <el-checkbox v-model="form.rule1" label="替换掉连续的空格、换行符和制表符" style="width: 100%" />
                                    <el-checkbox v-model="form.rule2" label="删除所有 URL 和电子邮箱地址" style="width: 100%" />
                                </el-form-item>
                            </el-form>
                        </el-card>

                    </el-form>
                    <div v-if="active==2">
                        <el-result icon="success" title="操作成功" sub-title="正在异步处理数据，请稍后...">
                        </el-result>
                    </div>
                    <el-button v-if="active>0 && active<2" @click="pre" :disabled="submitLoading">上一步</el-button>
                    <el-button v-if="active<1" :disabled="!this.form.documentId" type="primary" @click="next">下一步</el-button>
                    <el-button v-if="active==1" type="primary" @click="submit" :loading="submitLoading">提交</el-button>
                </el-col>
            </el-row>
        </el-main>
    </el-dialog>
</template>

<script>
export default {
    name: 'addDocument',
    data() {
        return {
            uploadApi: this.$API.system.file.upload,
            visible: false,
            active: 0,
            form: {
                documentId: null,
                splitOptions: [],
                splitType: 0,
                maxTokensPerLine: 500,
                overlap: 10,
            },
            splitOptions: [
                { value: '\r\n', label: '换行' },
                { value: '.', label: '句号' },
                { value: '?!', label: '问号或感叹号' },
                { value: ';', label: '分号' },
                { value: ':', label: '冒号' },
                { value: ',', label: '逗号' },
                { value: ')]}', label: '右括号' },
                { value: ' ', label: '空格' },
                { value: '-', label: '连字符' },
                { value: '\n\r', label: '换行符' },
                { value: null, label: '无分隔符' },
            ],

            rules: {
                documentId: [
                    {
                        required: true,
                        message: '请先上传文件',
                    },
                ],
            },
        };
    },
    mounted() {},
    methods: {
        open() {
            this.visible = true;
            return this;
        },
        //下一步
        next() {
            const formName = `stepForm_${this.active}`;
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.active += 1;
                } else {
                    return false;
                }
            });
        },
        //上一步
        pre() {
            this.active -= 1;
        },
        //提交
        submit() {
            const formName = `stepForm_${this.active}`;
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.submitLoading = true;
                    setTimeout(() => {
                        this.submitLoading = false;
                        this.active += 1;
                    }, 1000);
                } else {
                    return false;
                }
            });
        },
        //再来一次
        again() {
            this.active = 0;
        },
    },
    computed: {
        splitType0Class() {
            if (this.form?.splitType === 0) {
                return 'activateSplitType';
            }
            return '';
        },
        splitType1Class() {
            if (this.form?.splitType === 1) {
                return 'activateSplitType';
            }
            return '';
        },
    },
};
</script>

<style scoped>
.el-steps:deep(.is-finish) .el-step__line {
    background: var(--el-color-primary);
}
.activateSplitType {
    border-color: blue;
}
</style>
