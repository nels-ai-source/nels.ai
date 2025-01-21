<template>
    <el-container>
        <el-header>
            <div class="left-panel">
                <el-button type="primary" icon="el-icon-plus" @click="handleAddDocument" text>{{ $t("knowledgeDocument.btnAdd") }}</el-button>
            </div>
        </el-header>
        <el-main class="nopadding">
            <el-container>
                <el-aside style="width: 300px;" v-loading="isLoading">
                    <el-container>
                        <el-header>
                            <el-input placeholder="输入关键字进行过滤" v-model="dicFilterText" clearable></el-input>
                        </el-header>
                        <el-main class="nopadding">
                            <el-tree ref="document" class="menu" node-key="id" :data="documents" :props="documentProps" :current-node-key="activeDocument.id" :highlight-current="true" :expand-on-click-node="false" :filter-node-method="groupFilterNode" @node-click="handleNodeClick"></el-tree>
                        </el-main>
                    </el-container>
                </el-aside>
                <el-main class="nopadding">
                    <el-container>
                        <el-header>
                            <div class="left-panel">
                                <el-icon style="margin-right: 5px;"> <el-icon-document /> </el-icon>
                                {{ activeDocument.name  }}
                                <el-button icon="el-icon-edit" size="small" text @click="handleEditDocument" />
                            </div>
                            <div class="right-panel">
                                <el-button icon="el-icon-delete" size="small" text @click="handleDeteleDocument" />
                            </div>
                        </el-header>
                        <el-main>
                            <el-card v-for="paragraph in documentParagraphs" v-bind:key="paragraph.id" shadow="hover" style="margin-bottom: 10px;">
                                <template #header>
                                    <div class="el-header" style="padding: 0px;height: 30px; font-size: 12px;">
                                        <div class="left-panel">
                                            # {{ paragraph.index+1  }} ({{ paragraph.content.length }} 字符)
                                        </div>
                                        <div class="right-panel">
                                            <el-button-group class="ml-4">
                                                <el-button icon="el-icon-edit" size="small" text @click="handleEditParagraph(paragraph)" />
                                                <el-button icon="el-icon-delete" size="small" text />
                                            </el-button-group>
                                        </div>
                                    </div>

                                </template>
                                {{ paragraph.content }}
                            </el-card>
                        </el-main>
                    </el-container>
                </el-main>
            </el-container>

        </el-main>
    </el-container>
    <add-document-dialog v-if="dialog.addDocument" ref="addDocumentDialog" @success="getList" @closed="dialog.save=false"></add-document-dialog>
    <document-paragraph-dialog v-if="dialog.documentParagraph" ref="documentParagraphDialog" @success="getParagraphList" @closed="dialog.save=false"></document-paragraph-dialog>

</template>
<script>
import addDocumentDialog from './addDocument';
import documentParagraphDialog from './documentParagraph';
import { documentTypeMap } from '@/utils/documentTypeMap';
import { boolTypeMap } from '@/utils/commonTypeMap';
import { ElMessageBox } from 'element-plus';

export default {
    name: 'document',
    components: {
        addDocumentDialog,
        documentParagraphDialog,
    },
    data() {
        return {
            isLoading: false,
            isParagraphLoading: false,
            dialog: {
                addDocument: false,
                documentParagraph: false,
            },
            documentTypeMap,
            boolTypeMap,
            documents: [],
            activeDocument: { id: '' },
            documentProps: {
                label: 'name',
            },

            documentParagraphs: [],
        };
    },
    async mounted() {
        this.getList();
    },
    methods: {
        async getList() {
            this.isLoading = true;
            try {
                var res = await this.$API.aigc.knowledgeDocument.list.post({
                    knowledgeId: this.$route.query.id,
                });
                this.documents = res;
                if (this.documents.length > 0) {
                    this.activeDocument = this.documents[0];
                    await this.getParagraphList();
                }
            } finally {
                this.isLoading = false;
            }
        },
        handleAddDocument() {
            this.dialog.addDocument = true;
            this.$nextTick(() => {
                this.$refs.addDocumentDialog.open();
            });
        },
        async handleNodeClick(item) {
            this.activeDocument = item;
            await this.getParagraphList();
        },
        async handleDeteleDocument() {
            this.$confirm(
                this.$t('form.confirmDelete', {
                    name: this.activeDocument.name,
                }),
                this.$t('form.delete'),
                {
                    type: 'warning',
                    confirmButtonText: this.$t('form.delete'),
                    confirmButtonClass: 'el-button--danger',
                }
            )
                .then(async () => {
                    await this.$API.aigc.knowledgeDocument.delete.post(
                        this.activeDocument.id
                    );
                    await this.getList();
                })
                .catch(() => {});
        },
        async getParagraphList() {
            this.isParagraphLoading = true;
            try {
                var res =
                    await this.$API.aigc.knowledgeDocument.paragraphList.post({
                        knowledgeDocumentId: this.activeDocument.id,
                    });
                this.documentParagraphs = res;
            } finally {
                this.isParagraphLoading = false;
            }
        },
        async handleEditDocument() {
            ElMessageBox.prompt('', '重命名', {
                confirmButtonText: 'OK',
                cancelButtonText: 'Cancel',
                inputValue: this.activeDocument.name,
            })
                .then(async ({ value }) => {
                    try {
                        await this.$API.aigc.knowledgeDocument.uodateKnowledgeDocumentName.post(
                            {
                                knowledgeDocumentId: this.activeDocument.id,
                                name: value,
                            }
                        );
                    } finally {
                        this.activeDocument.name = value;
                    }
                })
                .catch(() => {});
        },

        handleEditParagraph(paragraph) {
            this.dialog.documentParagraph = true;
            this.$nextTick(() => {
                this.$refs.documentParagraphDialog.open().setData(paragraph);
            });
        },
    },
};
</script>