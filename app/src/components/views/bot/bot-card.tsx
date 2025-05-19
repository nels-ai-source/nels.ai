import React, { useContext } from 'react';
import { Card, Tag, Button, Dropdown, Modal } from 'antd';
import { StarOutlined, MoreOutlined } from '@ant-design/icons';
import { appContext } from '../../../hooks/provider';
import { useTranslation } from 'react-i18next';
import type { Bot } from '../../types/bot';
import { UUID } from 'crypto';

interface BotCardProps {
    bot: Bot;
    onCreateGallery: (bot: Bot) => void;
    onDeleteGallery: (botId: UUID) => void;
}

export const BotCard: React.FC<BotCardProps> = ({
    bot,
    onCreateGallery,
    onDeleteGallery,
}) => {
    const { user } = useContext(appContext);
    const { Meta } = Card;
    const { t } = useTranslation();
    return (
        <Card key={bot.id} className="group">
            <Meta
                avatar={
                    <img
                        src={`/images/bot/${bot.icon}`}
                        className="w-16 h-16 rounded-lg object-cover"
                    />
                }
                title={bot.name}
                description={
                    <>
                        <div className="h-[40px] line-clamp-2 overflow-hidden mb-2">
                            {bot.description}
                        </div>
                    </>
                }
            />
            <>
                <Tag>{t('bot.tag')}</Tag>
                <div className="flex items-center justify-between text-xs text-secondary">
                    <div className="flex items-center gap-x-1">
                        <span>{user?.name?.slice(0, 4)}</span>
                        <span className="w-1 h-1 rounded-full bg-secondary"></span>
                        <span>
                            {t('bot.lastEdit')}{' '}
                            {new Date(
                                bot.lastModificationTime ?? ''
                            ).toLocaleDateString(undefined, {
                                year: '2-digit',
                                month: '2-digit',
                                day: '2-digit',
                                hour: '2-digit',
                                minute: '2-digit',
                            })}
                        </span>
                    </div>
                    <div className="flex items-center gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                        <Button
                            type="text"
                            icon={<StarOutlined />}
                            className="hover:text-primary"
                        />
                        <Dropdown
                            menu={{
                                items: [
                                    {
                                        key: 'duplicate',
                                        label: t('bot.actions.duplicate'),
                                        onClick: () => onCreateGallery(bot),
                                    },
                                    {
                                        key: 'delete',
                                        label: t('bot.actions.delete'),
                                        danger: true,
                                        onClick: () => {
                                            Modal.confirm({
                                                title: t(
                                                    'bot.deleteConfirm.title'
                                                ),
                                                content: t(
                                                    'bot.deleteConfirm.content'
                                                ),
                                                okText: t(
                                                    'bot.actions.confirm'
                                                ),
                                                cancelText:
                                                    t('bot.actions.cancel'),
                                                onOk: () =>
                                                    onDeleteGallery(bot.id),
                                            });
                                        },
                                    },
                                ],
                            }}
                        >
                            <Button
                                type="text"
                                icon={<MoreOutlined />}
                                className="hover:text-primary"
                            />
                        </Dropdown>
                    </div>
                </div>
            </>
        </Card>
    );
};

export default BotCard;
