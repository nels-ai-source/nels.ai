import * as React from 'react';
import Layout from '../components/layout';
import { graphql } from 'gatsby';
import { BotManager } from '../components/views/bot/manager';

const BotPage = ({ data }: any) => {
    return (
        <Layout meta={data.site.siteMetadata} title="Home" link={'/bot'}>
            <main style={{ height: '100%' }} className=" h-full ">
                <BotManager />
            </main>
        </Layout>
    );
};

export const query = graphql`
    query ($language: String!) {
        site {
            siteMetadata {
                description
                title
            }
        }
        locales: allLocale(
            filter: { ns: { in: ["bot"] }, language: { eq: $language } }
        ) {
            edges {
                node {
                    ns
                    data
                    language
                }
            }
        }
    }
`;

export default BotPage;
