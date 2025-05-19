import React, { useState, useEffect, ReactNode } from 'react';
import { Button, message, Skeleton, List } from 'antd';
import {
    SettingOutlined,
    FileTextOutlined,
    FormOutlined,
    DeleteOutlined,
} from '@ant-design/icons';
import { useSearchParams } from 'react-router-dom';
import {
    Knowledge,
    KnowledgeDocument,
    KnowledgeDocumentParagraph,
} from '../../../types/knowledge';
import { v4 as uuidv4 } from 'uuid';
import { UUID } from 'crypto';
import { Header } from './header';
import { DocumentList } from './documentList';
import './detail.css';

const DocumentHeader: React.FC<{
    document: KnowledgeDocument | null;
}> = ({ document }) => (
    <header className="border-b border-gray-200 z-10 flex items-center justify-between h-14 px-2 md:h-14 md:px-4 shadow-sm">
        <div className="flex items-center space-x-2">
            <Button type="text" icon={<FileTextOutlined />} />
            {document?.name}
            <Button type="text" icon={<FormOutlined />} onClick={() => {}} />
        </div>

        <div className="flex items-center space-x-3">
            <>
                <Button
                    type="text"
                    size="small"
                    icon={<SettingOutlined />}
                    title="查看或调整配置"
                    onClick={() => {}}
                />
                <Button
                    type="text"
                    size="small"
                    icon={<DeleteOutlined />}
                    title="删除文档"
                    onClick={() => {}}
                />
            </>
        </div>
    </header>
);

const DocumentContent: React.FC<{
    document: KnowledgeDocument | null;
}> = ({ document }) => (
    <main className="flex-1 overflow-y-auto p-4 md:p-4">
        <List
            size="small"
            dataSource={document?.paragraphs}
            split={false}
            renderItem={(item) => (
                <List.Item
                    className={`
                        cursor-pointer 
                        rounded-lg 
                        p-2 
                        transition-colors 
                        mb-2
                        bg-gray-200 
                        relative 
                        group
                        hover:bg-gray-300
                    `}
                >
                    <div className="flex items-center p-2">
                        <div className="flex-1">
                            <div className="text-sm text-gray-600 leading-relaxed">
                                {item.content}
                            </div>
                        </div>
                        <div className="absolute top-2 right-2 flex items-center gap-1 invisible group-hover:visible transition-all duration-200">
                            <Button
                                type="text"
                                size="small"
                                icon={<FormOutlined />}
                                className="text-gray-500 hover:text-blue-500"
                                title="编辑段落"
                            />
                            <Button
                                type="text"
                                size="small"
                                icon={<DeleteOutlined />}
                                className="text-gray-500 hover:text-red-500"
                                title="删除段落"
                            />
                            <Button
                                type="text"
                                size="small"
                                icon={<SettingOutlined />}
                                className="text-gray-500 hover:text-blue-500"
                                title="段落设置"
                            />
                        </div>
                    </div>
                </List.Item>
            )}
        />
    </main>
);

export function KnowledgeDetail() {
    const [messageApi, contextHolder] = message.useMessage();
    const [knowledge, setKnowledge] = useState<Knowledge | null>(null);
    const [selectedDocument, setSelectedDocument] =
        useState<KnowledgeDocument | null>(null);
    const [loading, setLoading] = useState(true);
    const [searchParams] = useSearchParams();
    const id = searchParams.get('id');

    useEffect(() => {
        const fetchBot = async () => {
            setKnowledge({
                id: uuidv4() as UUID,
                name: 'AutoGen Studio',
                description: 'AutoGen Studio Bot',
                icon: 'URL_ADDRESSars.githubusercontent.com/u/111594291?s=200&v=4',
                documents: [
                    {
                        id: uuidv4() as UUID,
                        name: '名称 (3).docx',
                        type: 'docx',
                        paragraphs: [
                            {
                                id: uuidv4() as UUID,
                                content: '段落1',
                            },
                            {
                                id: uuidv4() as UUID,
                                content: '段落2',
                            },
                        ],
                    },
                    {
                        id: uuidv4() as UUID,
                        name: '天际·基础数据管理V5.0操作手册.pdf',
                        type: 'pdf',
                        paragraphs: [
                            {
                                id: uuidv4() as UUID,
                                content: '段落1',
                            },
                            {
                                id: uuidv4() as UUID,
                                content: '段落2',
                            },
                        ],
                    },
                    {
                        id: uuidv4() as UUID,
                        name: '百炼系列手机产品介绍.docx',
                        type: 'docx',
                        paragraphs: [
                            {
                                id: uuidv4() as UUID,
                                content: '段落1',
                            },
                            {
                                id: uuidv4() as UUID,
                                content: '段落2',
                            },
                        ],
                    },
                ],
            });
            setLoading(false);
            // try {
            //     const response = await fetch(`/api/bots/${id}`);
            //     const data = await response.json();
            //     setBot(data);
            // } catch (error) {
            //     message.error('获取Bot数据失败');
            // } finally {
            //     setLoading(false);
            // }
        };

        if (id) {
            fetchBot();
        }
    }, [id]);
    useEffect(() => {
        if (knowledge?.documents && knowledge.documents.length > 0) {
            setSelectedDocument(knowledge.documents[0]);
        }
    }, [knowledge]);

    return loading ? (
        <div className="relative flex h-full w-full overflow-hidden">
            <Skeleton active />
        </div>
    ) : (
        <div className="relative flex flex-col h-full w-full overflow-hidden">
            {contextHolder}
            {/* Header */}
            <div className="flex-shrink-0">
                <Header
                    data={knowledge}
                    onPublish={() => {
                        console.log(knowledge);
                    }}
                />
            </div>
            {/* Main Content*/}
            <main className="flex-1 min-h-0 flex">
                {/* Left Sidebar */}
                <aside className="flex-shrink-0 w-[300px] overflow-y-auto bg-white border-r border-gray-200 shadow-sm p-4">
                    {selectedDocument && (
                        <DocumentList
                            documents={knowledge?.documents}
                            selectedDocId={selectedDocument.id}
                            onDocumentSelect={setSelectedDocument}
                        />
                    )}
                </aside>

                {/* Main Content Area */}
                <div className="flex-1 overflow-y-auto">
                    <DocumentHeader document={selectedDocument} />
                    <DocumentContent document={selectedDocument} />
                </div>
            </main>
        </div>
    );
}
export default KnowledgeDetail;
